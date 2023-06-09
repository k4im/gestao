global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading.Tasks;
global using autenticacao.service.Models;
global using Microsoft.EntityFrameworkCore;
global using autenticacao.service.Data;
global using Microsoft.AspNetCore.Identity;
global using System.ComponentModel.DataAnnotations;
global using autenticacao.service.Repository;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.AspNetCore.Authorization;
global using autenticacao.service.Models.UserController;
global using autenticacao.service.chaveManager;
global using System.Security.Claims;
global using System.IdentityModel.Tokens.Jwt;
global using System.Text;
global using Microsoft.IdentityModel.Tokens;
global using autenticacao.service.jwtManager;
global using autenticacao.service.Models.Tokens;
global using autenticacao.service.RefreshManagers;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using AutoMapper;
global using Microsoft.OpenApi.Models;
global using System.Reflection;
global using Swashbuckle.AspNetCore.Filters;
global using System.Text.RegularExpressions;
global using autenticacao.service.Exceptions;
global using autenticacao.service.Models.ValueObjects;
global using Serilog;
global using Serilog.Sinks.Graylog;
global using autenticacao.service.Logger;