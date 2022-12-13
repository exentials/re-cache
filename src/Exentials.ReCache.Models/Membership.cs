namespace RedCache.Models
{
    public class Membership
    {
        /// <summary>
        /// User name 
        /// </summary>
        public string Username { get; set; } = string.Empty;
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; } = string.Empty;
        /// <summary>
        /// Authorization roles
        /// </summary>
        public string[] Roles { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Restituisce l'appartenenza di un membro ad un ruolo
        /// </summary>
        /// <param name="role">ruolo di autorizzazione</param>
        /// <returns>Restituisce true o false se il membro appartiene al ruolo</returns>
        public bool HasRole(string role)
        {
            return Roles?.Contains(role) ?? false;
        }
    }
}
