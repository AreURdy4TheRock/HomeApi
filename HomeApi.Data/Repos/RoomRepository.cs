using System;
using System.Linq;
using System.Threading.Tasks;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using Microsoft.EntityFrameworkCore;

namespace HomeApi.Data.Repos
{
    /// <summary>
    /// Репозиторий для операций с объектами типа "Room" в базе
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        private readonly HomeApiContext _context;

        public RoomRepository(HomeApiContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получение списка комнат
        /// </summary>
        public async Task<Room[]> GetRooms()
        {
            return await _context.Rooms
                .ToArrayAsync();
        }

        /// <summary>
        ///  Найти комнату по имени
        /// </summary>
        public async Task<Room> GetRoomByName(string name)
        {
            return await _context.Rooms.Where(r => r.Name == name).FirstOrDefaultAsync();
        }

        public async Task<Room> GetRoomById(Guid Id)
        {
            return await _context.Rooms.Where(r => r.Id == Id).FirstOrDefaultAsync();
        }

        /// <summary>
        ///  Добавить новую комнату
        /// </summary>
        public async Task AddRoom(Room room)
        {
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
                await _context.Rooms.AddAsync(room);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Обновить информацию о комнате
        /// </summary>
        public async Task UpdateRoom(Room room, UpdateRoomQuery query)
        {
            var existedRoom = await _context.Rooms.Where(r => r.Id == room.Id).FirstOrDefaultAsync();

            if (existedRoom == null)
            {
                Console.WriteLine(room.Id + "NO");
                return;
            }

            existedRoom.Name = query.NewName;
            existedRoom.Area = query.NewArea;
            existedRoom.GasConnected = query.NewGasConnected;
            existedRoom.Voltage = query.NewVoltage;

            // Добавляем в базу 
            var entry = _context.Entry(existedRoom);
            if (entry.State == EntityState.Detached)
                _context.Rooms.Update(existedRoom);

            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// Удаление комнаты
        /// </summary>
        public async Task DeleteRoom(Room room)
        {
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
        }
    }
}