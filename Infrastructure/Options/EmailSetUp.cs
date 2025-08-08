using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Infrastructure.Options;

namespace Infrastructure.Options
{
    public class EmailSetUp(IConfiguration Configuration) : IConfigureOptions<EmailSettings>
    {
        private readonly IConfiguration _Configuration = Configuration;
        private readonly string SectionName = "EmailSetting";

        public void Configure(EmailSettings options)
        {
            _Configuration.GetSection(SectionName).Bind(options);

        }
    }
}
