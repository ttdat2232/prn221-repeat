using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() { }
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(Type entityNotFound, object id, Type classThrowException) : base(ThrowMessage(entityNotFound, id, classThrowException)) { }

        private static string ThrowMessage(Type entityNotFound, object id, Type classThrowException)
        {
            return new StringBuilder(classThrowException.Name).Append(": Not found ").Append(entityNotFound.Name).Append(" with ID: ").Append(id.ToString()).ToString();
        }
    }
}
