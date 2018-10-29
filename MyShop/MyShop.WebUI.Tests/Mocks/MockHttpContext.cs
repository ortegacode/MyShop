using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Security.Principal;

namespace MyShop.WebUI.Tests.Mocks
{
    public class MockHttpContext : HttpContextBase
    {
        private MockRequest request;
        private MockResponse response;
        private HttpCookieCollection cookies;
        private IPrincipal FakeUser;

        public MockHttpContext()
        {
            cookies = new HttpCookieCollection();
            this.request = new MockRequest(cookies);
            this.response = new MockResponse(cookies);
            //created the cookies and passed it thru the requests and responses here because when our classes are reading and writing cookies it needs to work with the same underlying list AKA in this case its always working with the same collection of cookies.
        }

        public override IPrincipal User
        {
            get
            {
                return this.FakeUser;
            }

            set
            {
                this.FakeUser = value;
            }
        }

        public override HttpRequestBase Request
        {
            get
            {
                return request;
            }
        }
        


        public override HttpResponseBase Response
        {
            get
            {
                return response;
            }
        }
       

    }

    public class MockResponse : HttpResponseBase
    {
        private readonly HttpCookieCollection cookies; // created a private collection of cookies

        public MockResponse(HttpCookieCollection cookies) // constructor that allows us to pass in the cookie collection that we want, so we can have direct control over the cookies in our test.
        {
            this.cookies = cookies;
        }

        public override HttpCookieCollection Cookies // allows us to override the httpResponseBase methods (HttpCookieCollection Cookies)
        {
            get
            {
                return cookies;
            }
        }
    }

    public class MockRequest : HttpRequestBase
    {
        private readonly HttpCookieCollection cookies; 

        public MockRequest(HttpCookieCollection cookies) 
        {
            this.cookies = cookies;
        }

        public override HttpCookieCollection Cookies 
        {
            get
            {
                return cookies;
            }
        }
    }

}
