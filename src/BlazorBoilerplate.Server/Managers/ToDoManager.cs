﻿using BlazorBoilerplate.Server.Middleware.Wrappers;
using BlazorBoilerplate.Shared.DataInterfaces;
using BlazorBoilerplate.Shared.Dto.Sample;
using static Microsoft.AspNetCore.Http.StatusCodes;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BlazorBoilerplate.Server.Managers
{
    public class ToDoManager : ITodoManager
    {
        private readonly IToDoStore _toDoStore;

        public ToDoManager(IToDoStore toDoStore)
        {
            _toDoStore = toDoStore;
        }

        public async Task<ApiResponse> Get()
        {
            try
            {
                var todos = _toDoStore.GetAll();
                return new ApiResponse(Status200OK, "Retrieved Todos", todos);
            }
            catch (Exception ex)
            {
                return new ApiResponse(Status400BadRequest, ex.Message);
            }
        }

        public async Task<ApiResponse> Get(long id)
        {
            try
            {
                var todo = _toDoStore.GetById(id);
                return new ApiResponse(Status200OK, "Retrieved Todo", todo);
            }
            catch (Exception e)
            {
                return new ApiResponse(Status400BadRequest, "Failed to Retrieve Todo");
            }
        }

        public async Task<ApiResponse> Create(TodoDto todoDto)
        {
            var todo = await _toDoStore.Create(todoDto);
            return new ApiResponse(Status200OK, "Created Todo", todo);
        }

        public async Task<ApiResponse> Update(TodoDto todoDto)
        {
            try
            {
                var todo = await _toDoStore.Update(todoDto);
                return new ApiResponse(Status200OK, "Updated Todo", todo);
            }
            catch (InvalidDataException dataException)
            {
                return new ApiResponse(Status400BadRequest, "Failed to update Todo");
            }
        }

        public async Task<ApiResponse> Delete(long id)
        {
            try
            {
                await _toDoStore.DeleteById(id);
                return new ApiResponse(Status200OK, "Soft Delete Todo");
            }
            catch (InvalidDataException dataException)
            {
                return new ApiResponse(Status400BadRequest, "Failed to update Todo");
            }
        }
    }
}
