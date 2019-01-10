using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace NFineCore.Support
{
    public class UpLoad
    {
        public UpLoad()
        {

        }

        /// <summary>
        /// 通过文件流上传文件方法
        /// </summary>
        /// <param name="byteData">文件字节数组</param>
        /// <param name="fileName">文件名</param>
        /// <param name="isThumbnail">是否生成缩略图</param>
        /// <param name="isWater">是否打水印</param>
        /// <returns>上传成功返回JSON字符串</returns>
        public string FileSaveAs(string webRootPath, byte[] byteData, string fileName, bool isThumbnail, bool isWater)
        {
            try
            {
                string fileExt = Path.GetExtension(fileName).Trim('.'); //文件扩展名，不含“.”
                string newFileName = Utils.GetRamCode() + "." + fileExt; //随机生成新的文件名
                string newThumbnailFileName = "thumb_" + newFileName; //随机生成缩略图文件名

                string uploadPath = GetUploadPath(); //本地上传目录相对路径
                string fullUpLoadPath = webRootPath + uploadPath;//本地上传目录的物理路径
                string newFilePath = uploadPath + newFileName; //本地上传后的路径
                string newThumbnailPath = uploadPath + newThumbnailFileName; //本地上传后的缩略图路径

                byte[] thumbData = null; //缩略图文件流

                ////检查文件字节数组是否为NULL
                if (byteData == null)
                {
                    return "{\"status\": 0, \"msg\": \"请选择要上传的文件！\"}";
                }
                ////检查文件扩展名是否合法
                if (!CheckFileExt(fileExt))
                {
                    return "{\"status\": 0, \"msg\": \"不允许上传" + fileExt + "类型的文件！\"}";
                }
                ////检查文件大小是否合法
                if (!CheckFileSize(fileExt, byteData.Length))
                {
                    return "{\"status\": 0, \"msg\": \"文件超过限制的大小！\"}";
                }

                int imgMaxHeight = 1080;
                int imgMaxWidth = 1920;
                int thumbnailHeight = 200;
                int thumbnailWidth = 300;
                string thumbnailMode = "Cut";
                ////如果是图片，检查图片是否超出最大尺寸，是则裁剪
                if (IsImage(fileExt) && (imgMaxHeight > 0 || imgMaxWidth > 0))
                {
                    byteData = Thumbnail.MakeThumbnailImage(byteData, fileExt, imgMaxWidth, imgMaxHeight);
                }
                ////如果是图片，检查是否需要生成缩略图，是则生成
                if (IsImage(fileExt) && isThumbnail && thumbnailWidth > 0 && thumbnailHeight > 0)
                {
                    thumbData = Thumbnail.MakeThumbnailImage(byteData, fileExt, thumbnailWidth, thumbnailHeight, thumbnailMode);
                }
                else
                {
                    newThumbnailPath = newFilePath; //不生成缩略图则返回原图
                }

                int waterMarkPosition = 9;
                int waterMarkImgQuality = 80;
                int waterMarkFontSize = 12;
                int waterMarkTransparency = 5;
                int waterMarkType = 2;
                string waterMarkText = "NFine";
                string waterMarkFont = "12";
                string waterMarkPic = "watermark.png";
                //如果是图片，检查是否需要打水印
                if (IsWaterMark(fileExt) && isWater)
                {
                    switch (waterMarkType)
                    {
                        case 1:
                            byteData = WaterMark.AddImageSignText(byteData, fileExt, waterMarkText, waterMarkPosition, waterMarkImgQuality, waterMarkFont, waterMarkFontSize);
                            break;
                        case 2:
                            byteData = WaterMark.AddImageSignPic(byteData, fileExt, waterMarkPic, waterMarkPosition, waterMarkImgQuality, waterMarkTransparency);
                            break;
                    }
                }
                string fileServer = "localhost";
                //string ossSecretId = _attachOptions.OssSecretId; 
                //string ossSecretKey = _attachOptions.OssSecretKey;
                //string ossEndPoint = _attachOptions.OssEndPoint;
                //string ossBucket = _attachOptions.OssBucket;
                //string ossDomain = _attachOptions.OssDomain;
                //分发不同的上传方式处理
                switch (fileServer)
                {
                    case "aliyun": //阿里云OSS对象存储
                        ////检查配置是否完善
                        //if (string.IsNullOrEmpty(ossSecretId) || string.IsNullOrEmpty(ossSecretKey) || string.IsNullOrEmpty(ossEndPoint))
                        //{
                        //    return "{\"status\": 0, \"msg\": \"文件上传配置未完善，无法上传\"}";
                        //}
                        ////初始化阿里云配置
                        //Api.Cloud.AliyunOss aliyun = new Api.Cloud.AliyunOss(ossEndPoint, ossSecretId, ossSecretKey);
                        //string result = string.Empty; //返回信息

                        ////保存主文件
                        //if (!aliyun.PutObject(byteData, ossBucket, newFilePath, ossDomain, out result))
                        //{
                        //    return "{\"status\": 0, \"msg\": \"" + result + "\"}";
                        //}
                        //newFilePath = result; //将地址赋值给新文件地址

                        ////保存缩略图文件
                        //if (thumbData != null)
                        //{
                        //    aliyun.PutObject(thumbData, ossBucket, newThumbnailPath, ossDomain, out result);
                        //}
                        break;

                    case "qcloud": //腾讯云COS对象存储

                        break;
                    default: //本地服务器
                        //检查本地上传的物理路径是否存在，不存在则创建
                        if (!Directory.Exists(fullUpLoadPath))
                        {
                            Directory.CreateDirectory(fullUpLoadPath);
                        }
                        //保存主文件
                        FileHelper.SaveFile(byteData, fullUpLoadPath + newFileName);

                        //保存缩略图文件
                        if (thumbData != null)
                        {
                            FileHelper.SaveFile(thumbData, fullUpLoadPath + newThumbnailFileName);
                        }
                        break;
                }

                //处理完毕，返回JOSN格式的文件信息
                return "{\"status\": 1, \"msg\": \"上传文件成功！\", \"name\": \""
                    + fileName + "\", \"path\": \"" + newFilePath + "\", \"thumb\": \""
                    + newThumbnailPath + "\", \"size\": " + byteData.Length + ", \"ext\": \"" + fileExt + "\"}";
            }
            catch
            {
                return "{\"status\": 0, \"msg\": \"上传过程中发生意外错误！\"}";
            }
        }

        /// <summary>
        /// 保存远程文件到本地
        /// </summary>
        /// <param name="sourceUri">URI地址</param>
        /// <returns>上传后的路径</returns>
        public string RemoteSaveAs(string sourceUri)
        {
            //if (!IsExternalIPAddress(sourceUri))
            //{
            //    return "{\"status\": 0, \"msg\": \"INVALID_URL\"}";
            //}
            //var request = HttpWebRequest.Create(sourceUri) as HttpWebRequest;
            //using (var response = request.GetResponse() as HttpWebResponse)
            //{
            //    if (response.StatusCode != HttpStatusCode.OK)
            //    {
            //        return "{\"status\": 0, \"msg\": \"Url returns " + response.StatusCode + ", " + response.StatusDescription + "\"}";
            //    }
            //    if (response.ContentType.IndexOf("image") == -1)
            //    {
            //        return "{\"status\": 0, \"msg\": \"Url is not an image\"}";
            //    }
            //    try
            //    {
            //        //byte[] byteData = FileHelper.ConvertStreamToByteBuffer(response.GetResponseStream());
            //        //return FileSaveAs(byteData, sourceUri, false, false);
            //    }
            //    catch (Exception e)
            //    {
            //        return "{\"status\": 0, \"msg\": \"抓取错误：" + e.Message + "\"}";
            //    }
            //}
            return "";
        }

        private string GetUploadPath()
        {
            string webPath = "/";
            string filePath = "upload";
            int fileSave = 1;
            string path = webPath + filePath + "/"; //站点目录+上传目录
            switch (fileSave)
            {
                case 1: //按年月日每天一个文件夹
                    path += DateTime.Now.ToString("yyyyMMdd");
                    break;
                default: //按年月/日存入不同的文件夹
                    path += DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.ToString("dd");
                    break;
            }
            return path + "/";
        }
        /// <summary>
        /// 检查是否为合法的上传文件
        /// </summary>
        private bool CheckFileExt(string _fileExt)
        {
            //检查危险文件
            string[] excExt = { "asp", "aspx", "ashx", "asa", "asmx", "asax", "php", "jsp", "htm", "html" };
            for (int i = 0; i < excExt.Length; i++)
            {
                if (excExt[i].ToLower() == _fileExt.ToLower())
                {
                    return false;
                }
            }
            string fileExtension = "gif,jpg,jpeg,png,bmp,rar,zip,doc,xls,txt,mpp";
            string videoExtension = "flv,mp3,mp4,avi";
            //检查合法文件
            string[] allowExt = (fileExtension + "," + videoExtension).Split(',');
            for (int i = 0; i < allowExt.Length; i++)
            {
                if (allowExt[i].ToLower() == _fileExt.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 检查文件大小是否合法
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        /// <param name="_fileSize">文件大小(B)</param>
        private bool CheckFileSize(string _fileExt, int _fileSize)
        {
            int imgSize = 10240;
            int videoSize = 102400;
            int attachSize = 51200;
            string videoExtension = "flv,mp3,mp4,avi";
            //将视频扩展名转换成ArrayList
            ArrayList lsVideoExt = new ArrayList(videoExtension.ToLower().Split(','));
            //判断是否为图片文件
            if (IsImage(_fileExt))
            {
                if (imgSize > 0 && _fileSize > imgSize * 1024)
                {
                    return false;
                }
            }
            else if (lsVideoExt.Contains(_fileExt.ToLower()))
            {
                if (videoSize > 0 && _fileSize > videoSize * 1024)
                {
                    return false;
                }
            }
            else
            {
                if (attachSize > 0 && _fileSize > attachSize * 1024)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 是否为图片文件
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        private bool IsImage(string _fileExt)
        {
            ArrayList al = new ArrayList();
            al.Add("bmp");
            al.Add("jpeg");
            al.Add("jpg");
            al.Add("gif");
            al.Add("png");
            if (al.Contains(_fileExt.ToLower()))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否需要打水印
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        private bool IsWaterMark(string _fileExt)
        {
            var waterMarkType = 0;
            int.TryParse("2", out waterMarkType);
            //判断是否开启水印
            if (waterMarkType > 0)
            {
                //判断是否可以打水印的图片类型
                ArrayList al = new ArrayList();
                al.Add("bmp");
                al.Add("jpeg");
                al.Add("jpg");
                al.Add("png");
                if (al.Contains(_fileExt.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
