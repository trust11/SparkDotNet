#nullable enable

using System.Runtime.Serialization;

namespace SparkDotNet.Models
{
    public enum BehaviorType
    {
        //Calling in Webex(formerly Spark Call), or Hybrid Calling.
        [EnumMember(Value = "NATIVE_WEBEX_TEAMS_CALLING")]
        NATIVE_WEBEX_TEAMS_CALLING,

        //Cisco Jabber app
        [EnumMember(Value = "CALL_WITH_APP_REGISTERED_FOR_CISCOTEL")]
        CALL_WITH_APP_REGISTERED_FOR_CISCOTEL,

        //Third-Party app
        [EnumMember(Value = "CALL_WITH_APP_REGISTERED_FOR_TEL")]
        CALL_WITH_APP_REGISTERED_FOR_TEL,

        //Webex Calling app
        [EnumMember(Value = "CALL_WITH_APP_REGISTERED_FOR_WEBEXCALLTEL")]
        CALL_WITH_APP_REGISTERED_FOR_WEBEXCALLTEL,

        //Calling in Webex (Unified CM)
        [EnumMember(Value = "NATIVE_SIP_CALL_TO_UCM")]
        NATIVE_SIP_CALL_TO_UCM,

        ////Using the non-string value of null results in the organization-wide default calling behavior being in effect.
        //[EnumMember(Value = null)]
        //Null
    }

    public enum EffectiveBehaviorType
    {
        //Calling in Webex(formerly Spark Call), or Hybrid Calling.
        [EnumMember(Value = "NATIVE_WEBEX_TEAMS_CALLING")]
        NATIVE_WEBEX_TEAMS_CALLING,

        //Cisco Jabber app
        [EnumMember(Value = "CALL_WITH_APP_REGISTERED_FOR_CISCOTEL")]
        CALL_WITH_APP_REGISTERED_FOR_CISCOTEL,

        //Third-Party app
        [EnumMember(Value = "CALL_WITH_APP_REGISTERED_FOR_TEL")]
        CALL_WITH_APP_REGISTERED_FOR_TEL,

        //Webex Calling app
        [EnumMember(Value = "CALL_WITH_APP_REGISTERED_FOR_WEBEXCALLTEL")]
        CALL_WITH_APP_REGISTERED_FOR_WEBEXCALLTEL,

        //Calling in Webex (Unified CM)
        [EnumMember(Value = "NATIVE_SIP_CALL_TO_UCM")]
        NATIVE_SIP_CALL_TO_UCM,
    }

    public class PersonUcProfileSetting : PersonUcProfileSettingConfig
    {
        /// <summary>
        /// The effective Calling Behavior setting for the person, will be the organization's default Calling Behavior if the user's behaviorType is set to null.
        /// </summary>
        public EffectiveBehaviorType EffectiveBehaviorType { get; set; } = EffectiveBehaviorType.NATIVE_WEBEX_TEAMS_CALLING;
    }

    public class PersonUcProfileSettingConfig : WebexObject
    {
        /// <summary>
        /// The current Calling Behavior setting for the person. If null, the effective Calling Behavior will be the Organization's current default.
        /// </summary>
        public BehaviorType? BehaviorType { get; set; }

        /// <summary>
        /// A unique identifier for the person uc profile setting.
        /// </summary>
        public string? ProfileId { get; set; }
    }
}