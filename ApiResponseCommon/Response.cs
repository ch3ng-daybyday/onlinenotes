using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiResponseCommon
{
    class Response<T> : ResponseBase
    {
        public T Data { get; set; }//成功时返回的数据，如用户信息、查询结果等
    }
}
