using DatabaseUtility.EntityLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Util;
using Util.Entity;

namespace EntityLibrary
{
    public class OrderService
    {
        public PostResult<OrderEntity> InsertUserOrder(OrderEntity order)
        {
            PostResult<OrderEntity> resutl = new PostResult<OrderEntity>();
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    Order newOrder = new Order
                    {
                        PaymentMethods = order.PaymentMethods,
                        TruckingCode = order.TruckingCode,
                        UserID = order.UserID,
                        CreatedDate = DateTime.Now
                    };
                    decimal truckingAmount = entity.Trucking.Where(w => w.TruckingCode.Equals(order.TruckingCode)).FirstOrDefault().Amount;
                    newOrder.Amount = order.Amount + truckingAmount;
                    entity.Order.Add(newOrder);
                    entity.SaveChanges();
                    
                    if(order.OrderDetails.Count > 0)
                    {
                        List<OrderDetails> listOrderDetail = new List<OrderDetails>();
                        foreach(var it in order.OrderDetails)
                        {
                            listOrderDetail.Add(new OrderDetails
                            {
                                OrderId = newOrder.Id,
                                ProductCode = it.ProductCode,
                                Size = it.Size,
                                SizeValue = it.SizeValue,
                                Color = it.Color,
                                ColorValue = it.ColorValue,
                                Quantity = it.Quantity,
                                Amount = it.Amount,
                                CreatedDate = DateTime.Now,
                                Width = it.Width,
                                Height = it.Height
                            });
                        }
                        entity.OrderDetails.AddRange(listOrderDetail);
                        entity.SaveChanges();
                    }
                    resutl.Data = order;
                    resutl.Success = true;
                    resutl.ErrorCode = "0";
                    resutl.Message = "Thêm mới order thành công";
                }
            }
            catch (Exception ex)
            {
                resutl.Success = false;
                resutl.ErrorCode = "100";
                resutl.Message = "Thêm mới order lỗi: " + ex.Message;
            }
            return resutl;
        }

        public List<OrderViewEntity> GetAllOrder()
        {
            try
            {
                DataSet ds = new DataSet();
                ds = DatabaseAccessNonEntity.SearchStoreProcedure("sp_GetAllOrder");
                DataTable dtOrder = new DataTable();
                DataTable dtOrderDetail = new DataTable();
                dtOrder = ds.Tables[0];
                dtOrderDetail = ds.Tables[1];
                List<OrderViewEntity> list = new List<OrderViewEntity>();
                List<OrderDetailViewEntity> listDetail = new List<OrderDetailViewEntity>();

                foreach (DataRow row in dtOrderDetail.Rows)
                {
                    listDetail.Add(new OrderDetailViewEntity
                    {
                        Id= long.Parse(row[0].ToString()),
                        OrderId = long.Parse(row[1].ToString()),
                        ProductCode = row[2].ToString(),
                        SizeValue = row[4].ToString(),
                        ColorValue = row[7].ToString(),
                        Quantity = Convert.ToInt32(row[10].ToString()),
                        Amount = decimal.Parse(row[11].ToString()),
                        ProductName = row[14].ToString()
                    });
                }
                foreach(DataRow row in dtOrder.Rows)
                {
                    long id = long.Parse(row[0].ToString());
                    list.Add(new OrderViewEntity
                    {
                        Id = id,
                        OrderCode = row[1].ToString(),
                        SumAmount = decimal.Parse(row[2].ToString()),
                        CreatedDate = DateTime.Parse(row[3].ToString()),
                        FullName = row[4].ToString(),
                        PhoneNumber = row[5].ToString(),
                        Address = row[6].ToString(),
                        Description = row[7].ToString(),
                        TruckingAmount = decimal.Parse(row[8].ToString()),
                        PaymentMethods = row[9].ToString(),
                        Status = row[10].ToString(),
                        OrderDetails = listDetail.Where(w=>w.OrderId.Equals(id)).ToList()
                    });
                }
                return list;
            }
            catch (Exception)
            {
                return new List<OrderViewEntity>();
            }
        }
    }
}
