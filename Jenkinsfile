pipeline{
    agent any
    environment {
        dotnet = "C:\\Program Files (x86)\\dotnet\\"
    }
    stages{
        stage ('Clean workspace') {
            steps {
                cleanWs()
            }
        }

        stage('Checkout') {
            steps {
                git branch: 'Developer',
                    url: 'https://github.com/minhduc1582/SLN_API_PBL6.git'
            }
        }

        stage('Restore packages') {
            steps {
                bat "dotnet restore ${workspace}/Eshop_API/Eshop_API.csproj"
            }
        }
        stage('Clean') {
            steps {
                bat "dotnet clean ${workspace}/Eshop_API/Eshop_API.csproj"
            }
        }
        stage('Build') {
            steps {
                bat "dotnet build ${workspace}/Eshop_API/Eshop_API.csproj --configuration Release"
            }
        }
        stage('Test: Unit Test'){
            steps {
                bat "dotnet test ${workspace}/UnitTest_Eshop_API/UnitTest_Eshop_API.csproj"
            }
        }
            
        stage('Test: Integration Test'){
            steps {
            bat "dotnet test ${workspace}/UnitTest_Eshop_API/UnitTest_Eshop_API.csproj"
            }
        }
        stage('Publish'){
            steps{
            bat "dotnet publish ${workspace}/Eshop_API/Eshop_API.csproj"
            }
        }
    }
}