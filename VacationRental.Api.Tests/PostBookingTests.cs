using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Controllers;
using VacationRental.Api.Models;
using VacationRental.Domain;
using VacationRental.Domain.DomainObjects;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PostBookingTests
    {
        private readonly HttpClient _client;

        public PostBookingTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking_FirstCondition()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1
            };

            var postBookingRequest1 = new BookingBindingModel
            {
                Nights = 2,
                Start = new DateTime(2002, 01, 01)
            };

            await RequestSuccessed(postRentalRequest, postBookingRequest1);

            var postBooking2Request = new BookingBindingModel
            {
                Nights = 1,
                Start = new DateTime(2002, 01, 02)
            };

            await RequestFailed(postRentalRequest, postBookingRequest1);
        }


        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 4
            };
            var postBookingRequest1 = new BookingBindingModel
            {
                Nights = 3,
                Start = new DateTime(2001, 01, 01)
            };

            await RequestSuccessed(postRentalRequest, postBookingRequest1);          
        }

        /// <summary>
        /// This test just pass if run alone.
        /// </summary>
        /// <returns></returns>
        [Fact(Skip = "Just work if run alone")]
        //[Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking_SecondCondition()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 2
            };
            var postBookingRequest1 = new BookingBindingModel
            {
                Nights = 2,
                Start = new DateTime(2002, 01, 01)
            };

            await RequestSuccessed(postRentalRequest, postBookingRequest1);

            var postBookingRequest2 = new BookingBindingModel
            {
                RentalId = 5,
                Nights = 1,
                Start = new DateTime(2002, 01, 02)
            };
            postRentalRequest.Units = 3;

            await RequestSuccessed(postRentalRequest, postBookingRequest2);
        }

        /// <summary>
        /// This test just pass if run alone.
        /// </summary>
        /// <returns></returns>
        [Fact(Skip = "Just work if run alone")]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking_ThirdCondition()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 2
            };
            var postBookingRequest1 = new BookingBindingModel
            {
                RentalId = 4,
                Nights = 3,
                Start = new DateTime(2002, 01, 01)
            };

            await RequestSuccessed(postRentalRequest, postBookingRequest1);

            var postBookingRequest2 = new BookingBindingModel
            {
                RentalId = 5,
                Nights = 1,
                Start = new DateTime(2002, 01, 02)
            };
            postRentalRequest.Units = 3;

            await RequestSuccessed(postRentalRequest, postBookingRequest2);


            var postBookingRequest3 = new BookingBindingModel
            {
                RentalId = 3,
                Nights = 5,
                Start = new DateTime(2001, 12, 31)
            };
            postRentalRequest.Units = 3;

            await RequestSuccessed(postRentalRequest, postBookingRequest3);
        }

        protected async Task RequestSuccessed(RentalBindingModel postRentalRequest, BookingBindingModel postBookingRequest)
        {
            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            postBookingRequest.RentalId = postRentalResult.Id;

            ResourceIdViewModel postBookingResult;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getBookingResponse = await _client.GetAsync($"/api/v1/bookings/{postBookingResult.Id}"))
            {
                Assert.True(getBookingResponse.IsSuccessStatusCode);

                var getBookingResult = await getBookingResponse.Content.ReadAsAsync<BookingDto>();
                Assert.Equal(postBookingRequest.RentalId, getBookingResult.RentalId);
                Assert.Equal(postBookingRequest.Nights, getBookingResult.Nights);
                Assert.Equal(postBookingRequest.Start, getBookingResult.Start);
            }
        }

        protected async Task RequestFailed(RentalBindingModel postRentalRequest, BookingBindingModel postBookingRequest)
        {
            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            postBookingRequest.RentalId = postRentalResult.Id;

            ResourceIdViewModel postBookingResult;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingResult))
            {
                var errorDetails = await postBookingResponse.Content.ReadAsAsync<ErrorDetails>();
                Assert.False(postBookingResponse.IsSuccessStatusCode);
                Assert.Equal("Bad Request", postBookingResponse.ReasonPhrase);
            };
        }

    }
}
