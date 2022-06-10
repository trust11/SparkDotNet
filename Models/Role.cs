namespace SparkDotNet.Models
{
    /// <summary>
    /// A persona for an authenticated user, corresponding to a set of privileges within an organization.
    /// This roles resource can be accessed only by an admin.
    /// </summary>
    public class Role : WebexObject
    {
        /// <summary>
        /// A unique identifier for the role.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The name of the role.
        /// </summary>
        public string Name { get; set; }
    }
}