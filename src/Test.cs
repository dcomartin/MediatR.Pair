using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace MediatR.Pair
{
    public class Test
    {
        [Fact]
        public void Should_return_requests_with_no_handlers()
        {
            var noMatch = MediatorPair.FindUnmatchedRequests(Assembly.GetExecutingAssembly());
            noMatch.Count().ShouldBe(2);
            noMatch.SingleOrDefault(x => x == typeof(MyRequestWithoutHandler)).ShouldNotBeNull();
            noMatch.SingleOrDefault(x => x == typeof(MyRequestWithResultWithoutHandler)).ShouldNotBeNull();
        }
    }

    public class MyRequest : IRequest { }

    public class MyRequestHandler : IRequestHandler<MyRequest>
    {
        public Task Handle(MyRequest message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class MyRequestWithoutHandler : IRequest { }

    public class MyRequestWithResult : IRequest<bool> { }

    public class MyRequestWithResultHandler : IRequestHandler<MyRequestWithResult, bool>
    {
        public Task<bool> Handle(MyRequestWithResult request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class MyRequestWithResultWithoutHandler : IRequest<bool> { }

}
