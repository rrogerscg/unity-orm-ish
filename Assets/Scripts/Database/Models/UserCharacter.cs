using System;
using UnityEngine;

namespace ORMish
{
    public class UniqueUserCharacterNameError : Exception
    {
        public UniqueUserCharacterNameError() : base("User name must be unique.")
        {
        }

        public UniqueUserCharacterNameError(string message) : base(message)
        {
        }

        public UniqueUserCharacterNameError(string message, Exception innerException) : base(message, innerException)
        {
        }
    }


    [Serializable]
    public class UserCharacter : Record<UserCharacter>
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
        public UserCharacter() : base()
        {
        }

        // Create a new User object with a name
        // TODO: Hair, Sking, And Eyes SHOULD be a color object.  Lets make a json converted for this.
        // As of now, its a complex property with circular references and causes to a stack overflow error.
        // Converting to Vector4 does not work.
        public UserCharacter(string name, string hairColor, string skinColor, string eyeColor) : base()
        {
            if (!UserCharacterNameIsUnique(name))
            {
                throw new UniqueUserCharacterNameError("User name must be unique.");
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

        private static bool UserCharacterNameIsUnique(string name)
        {
            foreach (UserCharacter user in GetAll())
            {
                if (user.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            return true;
        }

        public static UserCharacter GetActiveCharacter()
        {
            foreach (UserCharacter user in GetAll())
            {
                if (user.IsActive)
                {
                    return user;
                }
            }
            return null;
        }

        public void SetAsActiveUserCharacter()
        {
            UserCharacter activeUserCharacter = GetActiveCharacter();
            if (activeUserCharacter != null)
            {
                activeUserCharacter.IsActive = false;
                activeUserCharacter.Put();
            }
            IsActive = true;
            Put();
        }
    }
}
