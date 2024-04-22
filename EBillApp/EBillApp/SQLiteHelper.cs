using System;
using SQLite;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EBillApp
{
    public class SQLiteHelper
    {
        SQLiteAsyncConnection db;
        public SQLiteHelper(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<RECORDS>().Wait();
        }

        //ADD and UPDATE records
        public Task<int> Save(RECORDS records)
        {
            if (records.METER_NUM != 0)
            {
                return db.UpdateAsync(records);
            }
            else
            {
                return db.InsertAsync(records);
            }
        }

        //DELETE
        public Task<int> Delete(RECORDS records)
        {
            return db.DeleteAsync(records);
        }

        // DISPLAY ALL or READ ALL 
        public Task<List<RECORDS>> DisplayAll()
        {
            return db.Table<RECORDS>().ToListAsync();
        }

        //SEARCH (specific)
        public Task<RECORDS> Search(int meter_num)
        {
          return db.Table<RECORDS>().Where(i => i.METER_NUM == meter_num).FirstOrDefaultAsync();
        }

    }
}
