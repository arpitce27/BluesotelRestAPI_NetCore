﻿using BluesotelRestAPI_NetCore.Models;
using BluesotelRestAPI_NetCore.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BluesotelRestAPI_NetCore
{
    public static class SeedData
    {
        // As static class doesn't have constructor 
        // we have added the dependency for our context class
        public static async Task InitializeAsync(IServiceProvider services)
        {
            await AddTestData(
                services.GetRequiredService<HotelApiDbContext>(),
                services.GetRequiredService<IDateLogicService>());
        }
        public static async Task AddTestData(HotelApiDbContext context, IDateLogicService dateLogicService)
        {
            if (context.Rooms.Any())
            {
                // Already has the data
                return;
            }
            var oxford = new RoomEntity
            {
                Id = Guid.Parse("301df04d-8679-4b1b-ab92-0a586ae53d08"),
                Name = "Oxford Suite",
                Rate = 10119,
            };
            context.Rooms.Add(oxford);

            context.Rooms.Add(new RoomEntity
            {
                Id = Guid.Parse("ee2b83be-91db-4de5-8122-35a9e9195976"),
                Name = "Driscoll Suite",
                Rate = 23959
            });

            // Adding some temp bookings
            var today = DateTimeOffset.Now;
            var start = dateLogicService.AlignStartTime(today);
            var end = start.Add(dateLogicService.GetMinimumStay());

            context.Bookings.Add(new BookingEntity
            {
                Id = Guid.Parse("2eac8dea-2749-42b3-9d21-8eb2fc0fd6bd"),
                Room = oxford,
                CreatedAt = DateTimeOffset.UtcNow,
                StartAt = start,
                EndAt = end,
                Total = oxford.Rate,
            });

            await context.SaveChangesAsync();
        }
    }
}
