using SparkDotNet.Models;

using System.Threading.Tasks;

namespace SparkDotNet
{
    public partial class Spark
    {
        private static string CallVoicemailUploadBusyGreetingBaseUrl { get; } = "/v1/people/{0}/features/voicemail/actions/uploadBusyGreeting/invoke";

        private static string CallVoicemailUploadNoAnswerGreetingBaseUrl { get; } = "/v1/people/{0}/features/voicemail/actions/uploadNoAnswerGreeting/invoke";

        private static string CallVoicemailUploadAnnouncementGreetingBaseUrl { get; } = "/v1/people/{0}/features/intercept/actions/announcementUpload/invoke";

        public async Task<SparkApiConnectorApiOperationResult> ConfigureBusyVoicemailUploadBusyGreetingSettingAsync(string personId, byte[] fileBytes, string contentName, string fileName, string orgId = null)
        {
            return await UploadAudioFile(CallVoicemailUploadBusyGreetingBaseUrl, personId, fileBytes, contentName, fileName, orgId).ConfigureAwait(false);
        }

        public async Task<SparkApiConnectorApiOperationResult> ConfigureBusyVoicemailUploadNoAnswerGreetingSettingAsync(string personId, byte[] fileBytes, string contentName, string fileName, string orgId = null)
        {
            return await UploadAudioFile(CallVoicemailUploadNoAnswerGreetingBaseUrl, personId, fileBytes, contentName, fileName, orgId).ConfigureAwait(false);
        }

        public async Task<SparkApiConnectorApiOperationResult> ConfigureBusyVoicemailUploadAnnouncementSettingAsync(string personId, byte[] fileBytes, string contentName, string fileName, string orgId = null)
        {
            return await UploadAudioFile(CallVoicemailUploadAnnouncementGreetingBaseUrl, personId, fileBytes, contentName, fileName, orgId).ConfigureAwait(false);
        }
    }
}