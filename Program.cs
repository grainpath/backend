using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace backend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var wapp = builder.Build();

        if (wapp.Environment.IsDevelopment())
        {
            wapp.UseSwagger()
                .UseSwaggerUI();
        }

        wapp.UseHttpsRedirection()
            .UseAuthorization();

        wapp.MapControllers();

        wapp.Run();
    }
}
