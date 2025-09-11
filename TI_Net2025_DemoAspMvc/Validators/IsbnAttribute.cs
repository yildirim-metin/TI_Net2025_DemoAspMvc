using System.ComponentModel.DataAnnotations;

namespace TI_Net2025_DemoAspMvc.Validators
{
    public class IsbnAttribute: ValidationAttribute
    {
        public IsbnAttribute()
        {
            ErrorMessage = "Isbn doit contenir 11 ou 13 caractère";
        }

        public override bool IsValid(object value)
        {
            if(value == null)
            {
                return true;
            }
            if(value is string isbn)
            {
                return isbn.Length == 11 || isbn.Length == 13;
            }
            return false;
        }
    }
}
