namespace IwbZero.Authorization.Permissions
{
    public class IwbPermissionGrantInfo
    {
        /// <summary>
        /// Name of the permission.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Is this permission granted Prohibited?
        /// </summary>
        public bool IsGranted { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="IwbPermissionGrantInfo"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isGranted"></param>
        public IwbPermissionGrantInfo(string name, bool isGranted)
        {
            Name = name;
            IsGranted = isGranted;
        }
    }
}
