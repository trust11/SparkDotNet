
using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SparkDotNet
{

    public partial class Spark
    {

        private readonly string meetingsTemplateBase = "/v1/meetings/templates";

        /// <summary>
        /// List all licenses for a given organization. If no orgId is specified, the default is the organization of the authenticated user.
        /// </summary>
        /// <param name="orgId">List licenses for this organization.</param>
        /// <param name="max"></param>
        /// <returns>List of License objects.</returns>
        public async Task<SparkApiConnectorApiOperationResult<MeetingTemplates>> GetMeetingTemplatesAsync(
            string templateType = null,
            string locale = null,
            bool? isDefault = null,
            bool? isStandard = null,
            string hostEmail = null,
            string siteUrl = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (templateType != null) queryParams.Add("templateType", templateType);
            if (locale != null) queryParams.Add("locale", locale);
            if (isDefault != null) queryParams.Add("isDefault", isDefault.ToString().ToLower());
            if (isStandard != null) queryParams.Add("isStandard", isStandard.ToString().ToLower());
            if (hostEmail != null) queryParams.Add("hostEmail", hostEmail);
            if (siteUrl != null) queryParams.Add("siteUrl", siteUrl);
            var path = GetURL(meetingsTemplateBase, queryParams);
            return await GetItemAsync<MeetingTemplates>(path);
        }

        /// <summary>
        /// Shows details for a license, by ID.
        /// Specify the license ID in the licenseId parameter in the URI.
        /// </summary>
        /// <param name="licenseId">The unique identifier for the license.</param>
        /// <returns>License object.</returns>
        public async Task<SparkApiConnectorApiOperationResult<MeetingTemplate>> GetMeetingTemplateAsync(string templateId, string hostEmail = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (hostEmail != null) queryParams.Add("hostEmail", hostEmail);
            var path = GetURL($"{meetingsTemplateBase}/{templateId}", queryParams);
            return await GetItemAsync<MeetingTemplate>(path);
        }
    }

}