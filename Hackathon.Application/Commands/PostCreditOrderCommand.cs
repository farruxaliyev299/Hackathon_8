using AutoMapper;
using AutoMapper.Internal.Mappers;
using Hackathon.Application.DTOs;
using Hackathon.Application.Mapping;
using Hackathon.Domain.Entities;
using Hackathon.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hackathon.Application.Commands
{
    public class PostCreditOrderCommand : IRequest<int>
    {
        public CreditDto Credit { get; set; }

        public PostCreditOrderCommand(CreditDto credit)
        {
            Credit = credit;
        }
    }

    public class PostCreditOrderCommandHandler : IRequestHandler<PostCreditOrderCommand, int>
    {
        private readonly HackathonDbContext _context;
        private static readonly HttpClient client = new HttpClient();
        public PostCreditOrderCommandHandler(HackathonDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(PostCreditOrderCommand request, CancellationToken cancellationToken)
        {
            var data = ObjectMapper.Mapper.Map<CreditApplication>(request.Credit);
            await _context.Applications.AddAsync(data);
            _context.SaveChanges();

            var result = JsonSerializer.Serialize(data);

            Dictionary<string, string> dict = new Dictionary<string, string>
            {
                {"data",result }
            };

            var a = await client.PostAsJsonAsync("http://91.102.161.166:5000/predict?data=" + dict["data"],"");
            var result2 = await  a.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Response>(result2).result;
        }
    }

    class Response
    {
        public int result { get; set; }
    }
}
