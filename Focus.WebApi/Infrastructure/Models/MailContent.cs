using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Infrastructure.Utilities;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class MailContent
    {
        public static implicit operator MailMessage(MailContent model)
        {
            if (!string.IsNullOrEmpty(model.FromAddress))
            {
                MailMessage mms = new MailMessage();
                mms.From = new MailAddress(model.FromAddress);
                mms.Subject = model.Subject;
                mms.SubjectEncoding = Encoding.UTF8;
                if (!model.IsHTML)
                {
                    mms.IsBodyHtml = model.IsHTML;
                    mms.Body = model.Body;
                    mms.BodyEncoding = Encoding.UTF8;
                }
                else
                {
                    //var TextView = AlternateView.CreateAlternateViewFromString(model.Body.GetPlainTextFromHTML(), Encoding.UTF8, "text/plain");
                    var TextView = AlternateView.CreateAlternateViewFromString(model.Body, Encoding.UTF8, "text/plain");
                    var HTMLview = AlternateView.CreateAlternateViewFromString(model.Body, Encoding.UTF8, "text/html");
                    mms.AlternateViews.Add(TextView);
                    mms.AlternateViews.Add(HTMLview);
                }

                //Add to Addresses
                if (!string.IsNullOrEmpty(model.ToAddresses))
                {
                    var addresses = MailHelper.GetAddressArray(model.ToAddresses);
                    foreach (var a in addresses)
                    {
                        mms.To.Add(new MailAddress(a));
                    }
                }
                //Add CC Addresses
                if (!string.IsNullOrEmpty(model.CcAddresses))
                {
                    var addresses = MailHelper.GetAddressArray(model.CcAddresses);
                    foreach (var a in addresses)
                    {
                        if (!string.IsNullOrEmpty(a))
                            mms.CC.Add(new MailAddress(a));
                    }
                }
                //Add BCC Addresses
                if (!string.IsNullOrEmpty(model.BccAddresses))
                {
                    var addresses = MailHelper.GetAddressArray(model.BccAddresses);
                    foreach (var a in addresses)
                    {
                        if (!string.IsNullOrEmpty(a))
                            mms.Bcc.Add(new MailAddress(a));
                    }
                }
                //Add ReplyTo Addresses
                if (!string.IsNullOrEmpty(model.ReplyToAddresses))
                {
                    var addresses = MailHelper.GetAddressArray(model.ReplyToAddresses);
                    foreach (var a in addresses)
                    {
                        if (!string.IsNullOrEmpty(a))
                            mms.ReplyToList.Add(new MailAddress(a));
                    }
                }
                //Add Attachments
                if (model.AttachmentPaths != null)
                {
                    foreach (var a in model.AttachmentPaths)
                    {
                        mms.Attachments.Add(new System.Net.Mail.Attachment(a.Trim()));
                    }
                }
                return mms;
            }
            return null;
        }
        /// <summary>
        /// 发件人邮箱地址
        /// </summary>
        public string FromAddress { get; set; }
        /// <summary>
        /// 收件人邮件地址
        /// </summary>
        public string ToAddresses { get; set; }
        /// <summary>
        /// 抄送地址
        /// </summary>
        public string? CcAddresses { get; set; }
        /// <summary>
        /// 密送地址
        /// </summary>
        public string? BccAddresses { get; set; }
        public string? ReplyToAddresses { get; set; }
        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 是否Html格式
        /// </summary>
        public bool IsHTML { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<string>? AttachmentPaths { get; set; }
    }
}
