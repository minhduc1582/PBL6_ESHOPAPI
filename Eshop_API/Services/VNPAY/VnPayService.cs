using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Eshop_API.Helpers.VNPAY;
using Eshop_API.Models.DTO.VNPAY;

namespace Eshop_API.Services.VNPAY
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        public VnPayService(IConfiguration configuration){
            _configuration = configuration;
        }

        public async Task<object> ChecksumReponse(NameValueCollection queryString)
        {
            string hashSecret = _configuration["Vnpay:HashSecret"]; //Chuỗi bí mật
                Paylib pay = new Paylib();

                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in queryString)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, queryString[s]);
                    }
                }

                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                // string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; //hash của dữ liệu trả về
                string vnp_SecureHash = queryString.Get("vnp_SecureHash"); //hash của dữ liệu trả về

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?
               
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        //Thanh toán thành công
                        return new {Message = "Confirm Success", RspCode = "00"};
                    }
                    else
                    {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        //ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                        return new {Message = "Confirm Success", RspCode = "00"};
                    }
                }
                else
                {
                    return new {Message = "Invalid Checksum", RspCode = "97"};
                    //ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
        }

        public async Task<string> CreateRequestUrl(ModelPayDto payInfo,string IpAddress)
        {
                string url = _configuration["Vnpay:BaseUrl"];
                string returnUrl = _configuration["Vnpay:ReturnUrl"];
                string tmnCode = _configuration["Vnpay:TmnCode"];
                string hashSecret = _configuration["Vnpay:HashSecret"];
                string Version = _configuration["Vnpay:Version"];
                string Locale = _configuration["Vnpay:Locale"];
                string Command = _configuration["Vnpay:Command"];
                string Ref_Bill = DateTime.Now.Ticks.ToString();
                Paylib pay = new Paylib();

                pay.AddRequestData("vnp_Version", Version); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
                pay.AddRequestData("vnp_Command", Command); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
                pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
                pay.AddRequestData("vnp_Amount", payInfo.Amount.ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
                pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
                pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
                pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
                pay.AddRequestData("vnp_IpAddr", IpAddress); //Địa chỉ IP của khách hàng thực hiện giao dịch
                pay.AddRequestData("vnp_Locale", Locale); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
                pay.AddRequestData("vnp_OrderInfo", payInfo.Content); //Thông tin mô tả nội dung thanh toán
                pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
                pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
                pay.AddRequestData("vnp_TxnRef", Ref_Bill); //mã hóa đơn
                pay.AddRequestData("vnp_Inv_Email", payInfo.Email); //địa chỉ email nhận hóa đơn
                                                                            
                string paymentUrl = pay.CreateRequestUrl(url, hashSecret);
                return paymentUrl;

        }
    }
}