﻿using InnoShop.Services.AuthAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;

namespace InnoShop.Services.AuthAPI.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            ResponseDto response;
            int statusCode;

            if (exception is DbUpdateException dbUpdateException)
            {
                statusCode = 500;
                response = new ResponseDto
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating the database.",
                    Errors = new List<string> { dbUpdateException.InnerException?.Message ?? dbUpdateException.Message }
                };
            }
            else if (exception is DbUpdateConcurrencyException)
            {
                statusCode = 409;
                response = new ResponseDto
                {
                    IsSuccess = false,
                    Message = "A concurrency conflict occurred while updating the database.",
                };
            }
            else if (exception is SqlException sqlException)
            {
                statusCode = 500;
                response = new ResponseDto
                {
                    IsSuccess = false,
                    Message = "A database error occurred.",
                    Errors = new List<string> { sqlException.Message }
                };
            }
            else
            {               
                statusCode = 500;
                response = new ResponseDto
                {
                    IsSuccess = false,
                    Message = "An unexpected error occurred.",
                    Errors = new List<string> { exception.Message }
                };
            }

            context.Result = new ObjectResult(response)
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true;
        }
    }
}
