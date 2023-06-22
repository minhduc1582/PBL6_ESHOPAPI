using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Entities;
using eshop_api.Helpers;
using Eshop_API.Helpers.Orders;
using Eshop_API.Helpers.VNPAY;
using Eshop_API.Models.DTO.VNPAY;
using Eshop_API.Repositories.Orders;
using Eshop_API.Repositories.VnPays;

namespace Eshop_API.Services.VNPAY
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        private readonly IOrderRepository _orderRepository;
        private readonly IBillPaymentRepository _billPaymentRepository;
        public VnPayService(IConfiguration configuration,
                            IOrderRepository orderRepository,
                            IBillPaymentRepository billPaymentRepository){
            _configuration = configuration;
            _orderRepository = orderRepository;
            _billPaymentRepository = billPaymentRepository;
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

            Guid orderId = Guid.Parse(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
            long vnp_Amount = Convert.ToInt64(pay.GetResponseData("vnp_Amount"))/100;
            long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
            string vnp_TransactionStatus = pay.GetResponseData("vnp_TransactionStatus");
            string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
            string vnp_OrderInfo = pay.GetResponseData("vnp_OrderInfo"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
            string vnp_PayDate = pay.GetResponseData("vnp_PayDate"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
            string vnp_BankCode = pay.GetResponseData("vnp_BankCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
            // string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; //hash của dữ liệu trả về
            string vnp_SecureHash = queryString.Get("vnp_SecureHash"); //hash của dữ liệu trả về

            bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

            object result;
            if (checkSignature)
            {
                var checkBill = await _billPaymentRepository.FirstOrDefault(x => x.TnxRef == orderId);
                //if(checkBill != null && checkBill.Status == PaymentStatus.Success) return;
                var order = await _orderRepository.FirstOrDefault(x => x.Id == orderId);
                BillPay billPay = new BillPay{
                    TnxRef = order.Id,
                    TransactionNo = vnpayTranId.ToString(),
                    Amount = order.Total.ToString(),
                    OrderInfo = vnp_OrderInfo,
                    PayDate = vnp_PayDate,
                    BankCode = vnp_BankCode
                };
                if (order.Total == vnp_Amount) {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        //Thanh toán thành công
                        billPay.Status = PaymentStatus.Success;
                        result =  new {Message = "Confirm Success", RspCode = "00"};
                    }
                    else
                    {
                        billPay.Status = PaymentStatus.Failed;
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        //ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                        result = new {Message = "Confirm Success", RspCode = "00"};
                    }
                }
                else{
                    billPay.Status = PaymentStatus.Failed;
                    result = new  {RspCode = "04", Message="invalid amount"};
                }
                await _billPaymentRepository.Add(billPay);
                await _billPaymentRepository.SaveChangesAsync();
            }
            else
            {
                //billPay.Status = PaymentStatus.Failed;
                result = new {Message = "Invalid Checksum", RspCode = "97"};
                //ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
            }
            //_billPaymentRepository.Add()
            return result;
        }

        public async Task<string> CreateRequestUrl(ModelPayDto payInfo,string IpAddress)
        {
              //  var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
                string url = _configuration["Vnpay:BaseUrl"];
                string returnUrl = payInfo.UrlOrigin;
                string tmnCode = _configuration["Vnpay:TmnCode"];
                string hashSecret = _configuration["Vnpay:HashSecret"];
                string Version = _configuration["Vnpay:Version"];
                string Locale = _configuration["Vnpay:Locale"];
                string Command = _configuration["Vnpay:Command"];
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
                pay.AddRequestData("vnp_TxnRef", payInfo.Tnx_Ref.ToString()); //mã hóa đơn
                pay.AddRequestData("vnp_Inv_Email", payInfo.Email); //địa chỉ email nhận hóa đơn
                pay.AddRequestData("vnp_Inv_Customer", payInfo.Name); //Họ tên của khách hàng in trên Hóa đơn điện tử
                
                                                                            
                string paymentUrl = pay.CreateRequestUrl(url, hashSecret);
                return paymentUrl;

        }
    }
}