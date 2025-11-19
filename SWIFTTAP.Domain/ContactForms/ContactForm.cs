using SWIFTTAP.Domain.Base;
using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Domain.Extensions;
using SWIFTTAP.Domain.Messages;

namespace SWIFTTAP.Domain.ContactForms;

public class ContactForm : Entity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Company { get; set; }
    public string Quantity { get; set; }
    public string Message { get; set; }

    public ContactForm SetName(string name)
    {
        if(name.IsEmpty())
            throw DomainException.FromErrorCode(ErrorCodes.ContactForm.NameInvalid);

        Name = name;
        return this;
    }

    public ContactForm SetEmail(string email)
    {
        if(email.IsEmpty())
            throw DomainException.FromErrorCode(ErrorCodes.ContactForm.EmailInvalid);

        Email = email;
        return this;
    }

    public ContactForm SetPhone(string phone)
    {
        if(phone.IsEmpty())
            throw DomainException.FromErrorCode(ErrorCodes.ContactForm.PhoneInvalid);

        Phone = phone;
        return this;
    }

    public ContactForm SetCompany(string company)
    {
        if(company.IsEmpty())
            throw DomainException.FromErrorCode(ErrorCodes.ContactForm.CompanyInvalid);

        Company = company;
        return this;
    }

    public ContactForm SetQuantity(string quantity)
    {
        if(quantity.IsEmpty())
            throw DomainException.FromErrorCode(ErrorCodes.ContactForm.QuantityInvalid);

        Quantity = quantity;
        return this;
    }

    public ContactForm SetMessage(string message)
    {
        if(message.IsEmpty())
            throw DomainException.FromErrorCode(ErrorCodes.ContactForm.MessageInvalid);

        Message = message;
        return this;
    }

    public static class Factory
    {
        public static ContactForm Create(string name,
                                          string email,
                                          string phone,
                                          string company,
                                          string quantity,
                                          string message,
                                          DateTime createdAt)
        {
            var contactForm = new ContactForm()
                .SetName(name)
                .SetEmail(email)
                .SetPhone(phone)
                .SetCompany(company)
                .SetQuantity(quantity)
                .SetMessage(message);

            contactForm.CreatedAt = createdAt;
            return contactForm;
        }
    }
}
