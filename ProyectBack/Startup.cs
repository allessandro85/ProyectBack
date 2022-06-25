using Microsoft.AspNetCore.Localization;
using ProyectIO;
using ProyectRepository;
using ProyectRepository.Conections;
using System.Globalization;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Xml;

namespace ProyectBack
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			#region log4net
			// Configuramos el log4net a la vieja usanza. Se podría inyectar como dependencia, pero lo dejamos para más adelante.
			XmlDocument log4netConfig = new XmlDocument();
			//using (var file = File.OpenRead("log4net.config"))
			//{
			//	log4netConfig.Load(file);
			//}

			
			#endregion

			#region servicios de la webAPI
			services.AddCors(options =>
			{
				options.AddDefaultPolicy(
					builder =>
					{
						builder.AllowAnyOrigin()
						.AllowAnyHeader()
						.AllowAnyMethod();
					});
			});

			//services.AddControllers();
			//services.AddControllers();

			//services.Configure<WebfeelSettings>(Configuration.GetSection("WebfeelSettings"));

			// configure jwt authentication
			//services.AddAuthentication(x =>
			//{
			//	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			//	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			//})
			//.AddJwtBearer(x =>
			//{
			//	x.RequireHttpsMetadata = false;
			//	x.SaveToken = true;
			//	x.TokenValidationParameters = new TokenValidationParameters
			//	{
			//		ValidateIssuerSigningKey = true,
			//		IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("TemporalSecretKey")),
			//		ValidateIssuer = false,
			//		ValidateAudience = false
			//	};
			//});

			// configure dependency injection for application services
			//services.AddScoped<IUserService, UserService>();

			services.AddMvc(option => option.EnableEndpointRouting = false)
				//.AddNewtonsoftJson(opt =>
				//{
				//    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
				//    opt.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
				//    opt.SerializerSettings.ContractResolver = new DefaultContractResolver
				//    {
				//        NamingStrategy = new DefaultNamingStrategy()
				//    };
				//})
				;

			IDbConnectionFactory dbConnectionFactory = new SqlConnectionFactory(Configuration.GetConnectionString("ProyectDB"));
			//var webfeelSettings = Configuration.GetSection("WebfeelSettings").Get<WebfeelSettings>();
			AppConfig.Instance.Set(dbConnectionFactory: dbConnectionFactory, appID: "", appName: "ProuectoDB");

			#endregion

			#region Swagger
			services.AddSwaggerGen(c =>
			{
				// Le avisamos a swagger que la API usa JWT Bearer token
				c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
				{
					Name = "Authorization",
					Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = Microsoft.OpenApi.Models.ParameterLocation.Header,
					Description = "<strong>Seguridad de esta API utilizando JWT.</strong><br/>Copiar la palabra 'Bearer' con un espacio y a continuación el Token de acceso."
				});

				c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
				{
					{
						new Microsoft.OpenApi.Models.OpenApiSecurityScheme
						{
							Reference = new Microsoft.OpenApi.Models.OpenApiReference
							{
								Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						new string[] {}
					}
				});

				// Set the comments path for the Swagger JSON and UI.
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				//c.IncludeXmlComments(xmlPath);

			});
			//services.AddSwaggerGenNewtonsoftSupport();

			#endregion
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			// Configuramos el manejo de excepciones
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				//app.UseExceptionHandler("/error");
				app.UseExceptionHandler(a => a.Run(context => MyExceptionHandler(context)));
			}

			// Configuramos Swagger
			if (env.IsDevelopment() || env.IsStaging())
			{

				app.UseSwagger();
				app.UseSwaggerUI(c =>
				{
					string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
					c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "DeclaracionesAPI");
				});
			}

			// Configuramos la localización
			var supportedCultures = new[]
			{
				new CultureInfo("es-AR"),
				new CultureInfo("pt"),
			};

			app.UseRequestLocalization(new RequestLocalizationOptions
			{
				DefaultRequestCulture = new RequestCulture("es-AR"),
				// Formatting numbers, dates, etc.
				SupportedCultures = supportedCultures,
				// UI strings that we have localized.
				SupportedUICultures = supportedCultures
			});

			//app.UseHttpsRedirection();

			app.UseRouting();

			app.UseCors();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}

		private static async Task MyExceptionHandler(HttpContext context)
		{

			var feature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
			var exception = feature.Error;

			string mensaje;
			int statusCode;

			if (exception is UnauthorizedAccessException)
			{
				mensaje = "";
				statusCode = StatusCodes.Status401Unauthorized;
			}

			//if (exception is BusinessException)
			//{
			//    // Si fue un error de capa de negocio, mostramos mensaje correspondiente
			//    mensaje = exception.Message;
			//    statusCode = 400;
			//}

			else
			{
				// Si fue una excepción real, logueamos y devolvemos mensaje genérico
				//var logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
				//logger.Error("Request: " + context.Request.ToString(), exception);

				mensaje = "Contacte soporte técnico"; // Textos.ErrorContacteSoporteTecnico;
				statusCode = StatusCodes.Status500InternalServerError;
			}

			//context.Response.ContentType = "application/json";
			context.Response.StatusCode = statusCode;
			await context.Response.WriteAsync(mensaje).ConfigureAwait(false);

		}
	}
}
