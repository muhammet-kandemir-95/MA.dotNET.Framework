using System;
using System.Collections.Generic;
using System.Text;

namespace MA.dotNET.Framework.Standart.ClassLibrary.MimeReader
{
    public class EmailMime
    {
        #region Constructors
        public EmailMime(string text)
        {
            this.Parameters = new Dictionary<string, string>();

            string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            StringBuilder originalBody = new StringBuilder();
            bool bodyStarted = false;
            bool bodyDisable = false;
            Encoding bodyEncoding = Encoding.ASCII;

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (line == ".")
                    break;
                if (bodyStarted == false)
                {
                    if (string.IsNullOrEmpty(line) == true)
                    {
                        bodyStarted = true;
                        bodyDisable = false;

                        var attributeContentType = this.Parameters["Content-Type"];
                        var splitAttributeContentType = attributeContentType.Split(';');
                        this.ContentType = splitAttributeContentType[0].Trim();
                        if (splitAttributeContentType.Length > 1)
                        {
                            for (int j = 1; j < splitAttributeContentType.Length; j++)
                            {
                                var attributeParameter = splitAttributeContentType[j];
                                var attributeParameterSplitIndex = attributeParameter.IndexOf('=');
                                string parameterName = attributeParameter.Substring(0, attributeParameterSplitIndex).Trim().ToLower();
                                string parameterValue = attributeParameter.Substring(attributeParameterSplitIndex + 1).Trim();

                                switch (parameterName)
                                {
                                    case "charset":
                                        bodyEncoding = Encoding.GetEncoding(parameterValue);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        var lineSplitIndex = line.IndexOf(':');
                        string parameterName = line.Substring(0, lineSplitIndex).TrimEnd();
                        string parameterValue = line.Substring(lineSplitIndex + 1).TrimStart();
                        this.Parameters.Add(parameterName, parameterValue);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(line) == true)
                        bodyDisable = false;
                    else if (bodyDisable == false)
                    {
                        switch (this.Parameters["Content-Transfer-Encoding"])
                        {
                            case "base64":
                                break;
                            default:
                                if (line[line.Length - 1] == '=')
                                    line = line.Substring(0, line.Length - 1);
                                break;
                        }
                        originalBody.Append(line);
                    }
                }
            }

            string originalBodyAsString = originalBody.ToString();
            originalBody.Clear();

            if (this.Parameters.ContainsKey("Subject"))
            {
                //=?utf-8?B?<BASE64>?=
                var subjectResult = this.Parameters["Subject"];
                if (subjectResult.StartsWith("=?"))
                {
                    var splitSubjectResult = subjectResult.Split('?');
                    var subjectEncoding = splitSubjectResult[1];
                    var subjectType = splitSubjectResult[2];
                    var subjectData = splitSubjectResult[3];

                    switch (subjectType)
                    {
                        case "B":
                            {
                                this.Subject = Encoding.GetEncoding(subjectEncoding).GetString(Convert.FromBase64String(subjectData));
                            }
                            break;
                        default:
                            this.Subject = subjectData;
                            break;
                    }
                }
                else
                    this.Subject = subjectResult;
            }

            if (this.Parameters.ContainsKey("Content-Transfer-Encoding"))
            {
                switch (this.Parameters["Content-Transfer-Encoding"])
                {
                    case "base64":
                        this.Body = bodyEncoding.GetString(Convert.FromBase64String(originalBodyAsString));
                        break;
                    default:
                        {
                            string bodyReader = originalBodyAsString;
                            StringBuilder bodyResult = new StringBuilder();
                            for (int i = 0; i < bodyReader.Length; i++)
                            {
                                char item = bodyReader[i];
                                switch (item)
                                {
                                    case '=':
                                        bodyResult.Append(Convert.ToChar(Convert.ToByte(bodyReader.Substring(i + 1, 2), 16)));
                                        i += 2;
                                        break;
                                    default:
                                        bodyResult.Append(item);
                                        break;
                                }
                            }
                            this.Body = bodyResult.ToString();
                            bodyResult.Clear();
                        }
                        break;
                }
            }
        }
        #endregion

        #region Variables
        public Dictionary<string, string> Parameters { get; private set; }

        public string ContentType { get; set; }
        public string Body { get; private set; }
        public string Subject { get; private set; }
        #endregion
    }
}
