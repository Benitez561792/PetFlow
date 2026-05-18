using Microsoft.EntityFrameworkCore;
using PetFlowAPI.Data;
using PetFlowAPI.Mappings;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "PetFlow API",
        Version = "v1",
        Description = "API para gerenciamento de saúde de pets - FIAP Challenge 2026 | Clyvo Vet"
    });
});

var connectionString = builder.Configuration.GetConnectionString("OracleConnection");
builder.Services.AddDbContext<PetFlowContext>(options =>
    options.UseOracle(connectionString)
           .EnableDetailedErrors()
           .EnableSensitiveDataLogging()
);

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PetFlow API v1");
    c.RoutePrefix = "swagger";
});

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";

        var exceptionFeature = context.Features
            .Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();

        if (exceptionFeature == null) return;

        var ex = exceptionFeature.Error;

        // Mensagens amigáveis para erros conhecidos do Oracle / EF Core
        var innerMsg = ex.InnerException?.Message ?? ex.Message;

        // ORA-00001: unique constraint — registro duplicado
        if (innerMsg.Contains("ORA-00001") || innerMsg.Contains("unique constraint"))
        {
            context.Response.StatusCode = 409;
            await context.Response.WriteAsJsonAsync(new
            {
                statusCode = 409,
                erro = "Registro duplicado.",
                mensagem = "Já existe um registro com esses dados. Verifique os campos únicos (ex: email, código) e tente novamente."
            });
            return;
        }

        // ORA-02292: integrity constraint — FK impede delete
        if (innerMsg.Contains("ORA-02292") || innerMsg.Contains("integrity constraint"))
        {
            context.Response.StatusCode = 409;
            await context.Response.WriteAsJsonAsync(new
            {
                statusCode = 409,
                erro = "Operação bloqueada por dependência.",
                mensagem = "Este registro possui dados relacionados e não pode ser removido ou alterado diretamente."
            });
            return;
        }

        // ORA-01400: campo obrigatório nulo
        if (innerMsg.Contains("ORA-01400") || innerMsg.Contains("cannot insert NULL"))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new
            {
                statusCode = 400,
                erro = "Campo obrigatório ausente.",
                mensagem = "Um ou mais campos obrigatórios não foram informados."
            });
            return;
        }

        // ORA-01438: valor maior que o permitido pela coluna
        if (innerMsg.Contains("ORA-01438"))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new
            {
                statusCode = 400,
                erro = "Valor fora do limite.",
                mensagem = "Um dos campos enviados excede o tamanho máximo permitido pelo banco de dados."
            });
            return;
        }

        // Erro genérico — não expõe stack trace em produção
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new
        {
            statusCode = 500,
            erro = "Erro interno no servidor.",
            mensagem = "Ocorreu um erro inesperado. Tente novamente em instantes."
        });
    });
});

app.UseAuthorization();
app.MapControllers();

app.Run("http://localhost:5000");
