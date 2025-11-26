using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;

namespace OrderService.Models
{
    public class PPProductGroup
    {
        public string name { get; set; }
        public string status { get; set; }
        public double? tonnage { get; set; }
        public double? ordered_tonnage { get; set; }
        public string actual_date { get; set; }
        public string required_date { get; set; }
        public string forecasting_date { get; set; }
        public double? forecasting_tonnage { get; set; }
    }
    public class PPParts
    {
        public string name { get; set; }
        public PPProductGroup[] productGroup { get; set; }
        public string value { get; set; }
    }
    public class PPStructureElement
    {
        public string name { get; set; }
        public int sequence { get; set; }
        public PPParts[] parts { get; set; }
    }
    public class PPStorey
    {
        public string storey { get; set; }
        public int sequence { get; set; }
        public PPStructureElement[] structureElements { get; set; }
    }
    public class PPWBS
    {
        public string block { get; set; }
        public int sequence { get; set; }
        public PPStorey[] stories { get; set; }
    }


    public class projectProgress
    {
        public ObjectId _id { get; set; }
        public string project { get; set; }
        public PPWBS[] wbs { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class WBSActualDelivery
    {
        public ObjectId _id { get; set; }
        public string projectId { get; set; }
        public string block { get; set; }
        public string storey { get; set; }
        public string part { get; set; }
        public string structureElement { get; set; }
        public string deliveryDate { get; set; }
        public string requiredDate { get; set; }
    }
    public class QueryResut
    {
        public ObjectId _id { get; set; }
        public string project { get; set; }
        public string block { get; set; }
        public string storey { get; set; }
        public string structureElement { get; set; }
        public string part { get; set; }
        public string product { get; set; }
        public string forecast { get; set; }

    }


}