using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtLabSingleProject.Dtos
{
    /// <summary>
    /// 呼叫 API 回傳的制式格式
    /// </summary>
    public class APIResult<T> : ICloneable, INotifyPropertyChanged
    {
        /// <summary>
        /// 此次呼叫 API 是否成功
        /// </summary>
        public bool Status { get; set; } = true;
        public int HTTPStatus { get; set; } = 200;
        public int ErrorCode { get; set; }
        /// <summary>
        /// 呼叫 API 失敗的錯誤訊息
        /// </summary>
        public string Message { get; set; } = "";
        /// <summary>
        /// 呼叫此API所得到的其他內容
        /// </summary>
        public T Payload { get; set; }

        #region 介面實作
        public event PropertyChangedEventHandler PropertyChanged;

        public APIResult<T> Clone()
        {
            return ((ICloneable)this).Clone() as APIResult<T>;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
