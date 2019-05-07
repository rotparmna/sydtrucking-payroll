namespace sydtrucking_payroll_front.business
{
    using System;

    public class Authenticate
    {
        private model.User _user;
        private User _userBusiness;

        public bool IsAuthenticate { get; private set; }
        public string Fullname
        {
            get
            {
                return _user.Fullname;
            }
        }
        public DateTime LastLogin { get; private set; }

        public Authenticate()
        {
            _userBusiness = new User();
            _user = new model.User();
            IsAuthenticate = false;
        }

        public void Login(string username, string password)
        {
            _user = new model.User()
            {
                Username = username,
                Password = password
            };

            if (_userBusiness.Validate(_user))
            {
                _user = _userBusiness.Get(_user);
                LastLogin = _user.LastLogin;
                _userBusiness.UpdateLastLogin(_user);
                IsAuthenticate = true;
            }
        }
    }
}
