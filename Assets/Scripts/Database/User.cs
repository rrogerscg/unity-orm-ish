using System;
using UnityEngine;

namespace ORMish
{
    public class UniqueUserNameError : Exception
    {
        public UniqueUserNameError() : base("User name must be unique.")
        {
        }

        public UniqueUserNameError(string message) : base(message)
        {
        }

        public UniqueUserNameError(string message, Exception innerException) : base(message, innerException)
        {
        }
    }


    [Serializable]
    public class User : Record<User>
    {

        [SerializeField]
        public string Name { get; set; }
        [SerializeField]
        public bool IsActive;
        [SerializeField]
        public string HairColor { get; set; }
        [SerializeField]
        public string SkinColor { get; set; }
        [SerializeField]
        public string EyeColor { get; set; }


        // Required constructor to use for generic type
        public User() : base()
        {
        }

        // Create a new User object with a name
        // TODO: Hair, Sking, And Eyes SHOULD be a color object.  Lets make a json converted for this.
        // As of now, its a complex property with circular references and causes to a stack overflow error.
        // Converting to Vector4 does not work.
        public User(string name, string hairColor, string skinColor, string eyeColor) : base()
        {
            if (!UserNameIsUnique(name))
            {
                throw new UniqueUserNameError("User name must be unique.");
            }
            Name = name;
            HairColor = hairColor;
            SkinColor = skinColor;
            EyeColor = eyeColor;
            IsActive = false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, CreationDate, IsActive, Name);
        }

        private static bool UserNameIsUnique(string name)
        {
            foreach (User user in GetAll())
            {
                if (user.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            return true;
        }

        public static User GetActiveUser()
        {
            foreach (User user in GetAll())
            {
                if (user.IsActive)
                {
                    return user;
                }
            }
            return null;
        }

        public void SetAsActiveUser()
        {
            User activeUser = GetActiveUser();
            if (activeUser != null)
            {
                activeUser.IsActive = false;
                activeUser.Put();
            }
            IsActive = true;
            Put();
        }
    }
}
