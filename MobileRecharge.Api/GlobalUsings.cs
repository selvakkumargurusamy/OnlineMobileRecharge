﻿global using System.Net;
global using MediatR;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;
global using MobileRecharge.Application.Command;
global using MobileRecharge.Application.HttpService;
global using MobileRecharge.Application.Interfaces;
global using MobileRecharge.Application.Mapper;
global using MobileRecharge.Application.Queries;
global using MobileRecharge.Application.Services;
global using MobileRecharge.Domain.Configuration;
global using MobileRecharge.Domain.Dtos;
global using MobileRecharge.Domain.Interfaces.Repositories;
global using MobileRecharge.Infrastructure.Database;
global using MobileRecharge.Infrastructure.Repositories;