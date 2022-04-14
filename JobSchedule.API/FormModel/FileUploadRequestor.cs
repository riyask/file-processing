using JobSchedule.API.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobSchedule.API.FormModel
{
    public class FileUploadRequestor
    {
        [AllowedExtensions(new string[] { ".txt" })]
        public IFormFile File { get; set; }
    }
}
