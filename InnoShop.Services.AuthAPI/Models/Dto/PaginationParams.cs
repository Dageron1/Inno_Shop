﻿namespace InnoShop.Services.AuthAPI.Models.Dto
{
    public class PaginationParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
