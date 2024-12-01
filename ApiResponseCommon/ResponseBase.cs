using System.Globalization;

namespace ApiResponseCommon
{

    public class ResponseBase
    {
        public string Status { get; set; } // "success" | "error" | "warning" | "info"
        public string Message { get; set; }//"操作成功" | "请求错误" | "需要验证" | ...,
        public ApiResponseMetadata Meta { get; set; }//额外的元数据，如分页信息、验证码等
        public string Token { get; set; }
    }

    public class ApiResponseMetadata
    {
        public PaginationMetadata Pagination { get; set; }//分页信息
        public CaptchaMetadata Captcha { get; set; }//验证码
        public UserVerificationMetadata User { get; set; }//用户

        public ApiResponseMetadata()
        {
            Captcha = new CaptchaMetadata()
            {
                Enabled = false,
                ImageUrl = null,
                Token = null
            };
        }
    }
    public class PaginationMetadata
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
    }

    public class CaptchaMetadata
    {
        public bool Enabled { get; set; }// 是否启用验证码
        public string Token { get; set; }// 验证码令牌（如果已生成）
        public string ImageUrl { get; set; }// 验证码图片的URL（如果适用）
    }


    public class UserVerificationMetadata
    {
        public bool Verified { get; set; } // 用户是否已验证
        public string Message { get; set; } // 是否需要用户进行额外的验证（如验证码）
    }
    public class Url
    {
        public string RedirectUrl { set; get; }
    }
}

