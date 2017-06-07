using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDD_Project
{
    public class Rentals : IRentals
    {
        List<Rental> rent = new List<Rental>();
        IDatateTime Idt;

        public Rentals(IDatateTime date)
        {
            Idt = date;
        }

        public void AddRental(string movieTitle, string socialSecurityNumber)
        {
            bool vvv = rent.Any(x => x.socialSecurityNumber.Equals(socialSecurityNumber) && x.Due_Date < Idt.Now());

            if (vvv)
            {
                throw new DueDateExpiredExeption();
            }

            rent.Add(new Rental()
            {
                socialSecurityNumber = socialSecurityNumber,
                Due_Date = Idt.Now().AddDays(3),
                Movie_Title = movieTitle
            });
        }

        public List<Rental> GetRentalsFor(string socialSecurityNumber)
        {
            List<Rental> listrent = rent.FindAll(x => x.socialSecurityNumber.Equals(socialSecurityNumber));

            return listrent;
        }

        public Rental GetRentalForMovie(string movieTitle, string socialSecurityNumber)
        {
            List<Rental> theRnts = rent.FindAll(x => x.Movie_Title.Equals(movieTitle));

            Rental theR = theRnts.Find(y => y.socialSecurityNumber.Equals(socialSecurityNumber));

            return theR;
        }

        public void RemoveRental(string movieTitle, string socialSecurityNumber)
        {
            List<Rental> theRnts = rent.FindAll(x => x.Movie_Title.Equals(movieTitle));

            Rental theR = theRnts.Find(y => y.Movie_Title.Equals(socialSecurityNumber));

            rent.Remove(theR);
        }
    }
}
