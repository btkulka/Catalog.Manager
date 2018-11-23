using CatalogData;
using CatalogData.Classes.CMOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerUI.Models.ResyncAnnotations
{
    public class CatalogItemAnnotation
    {

        public CatalogItem CatalogItem { get; set; }
        public string Annotation { get; set; }

        public CatalogItemAnnotation(CatalogItem cat, string a)
        {
            CatalogItem = cat;
            Annotation = a;
        }

        internal static bool HasBeenUpdated(CatalogItem cmc, CMOCatalogItem cmo)
        {
            bool updated = false;

            updated = updated | cmc.AdjFactor != (decimal)cmo.adjFactor;
            updated = updated | cmc.CII != (decimal)cmo.CII;
            updated = updated | cmc.ComponentId != cmo.ComponentId;
            updated = updated | cmc.ComponentTypeId != cmo.ComponentTypeId;
            updated = updated | cmc.ComponentUiiId != cmo.uiiId;
            updated = updated | cmc.MaterialCategoryId != cmo.MaterialCategoryId;
            updated = updated | cmc.UoM != cmo.UoM;
            updated = updated | cmc.UseAltDesc != cmo.useAltDesc;

            return updated;
        }

        internal static string GetUpdatedFields(CatalogItem cmc, CatalogItem updCmc)
        {
            string updateString = "Updated Fields: ";

            if (cmc.AdjFactor != updCmc.AdjFactor)
            {
                updateString += String.Format(" [AdjFactor {0} => {1}] ", cmc.AdjFactor, updCmc.AdjFactor);
            }

            if (cmc.CII != updCmc.CII)
            {
                updateString += String.Format(" [CII {0} => {1}] ", cmc.CII, updCmc.CII);
            }

            if (cmc.ComponentId != updCmc.ComponentId)
            {
                updateString += String.Format(" [ComponentId {0} => {1}] ", cmc.ComponentId, updCmc.ComponentId);
            }

            if (cmc.ComponentTypeId != updCmc.ComponentTypeId)
            {
                updateString += String.Format(" [ComponentTypeId {0} => {1}] ", cmc.ComponentTypeId, updCmc.ComponentTypeId);
            }

            if (cmc.ComponentUiiId != updCmc.ComponentUiiId)
            {
                updateString += String.Format(" [ComponentUiiId {0} => {1}] ", cmc.ComponentUiiId, updCmc.ComponentUiiId);
            }

            if (cmc.MaterialCategoryId != updCmc.MaterialCategoryId)
            {
                updateString += String.Format(" [MaterialCategoryId {0} => {1}] ", cmc.MaterialCategoryId, updCmc.MaterialCategoryId);
            }

            if (cmc.TaskListId != updCmc.TaskListId)
            {
                updateString += String.Format(" [TaskListId {0} => {1}] ", cmc.TaskListId, updCmc.TaskListId);
            }

            if (cmc.UoM != updCmc.UoM)
            {
                updateString += String.Format(" [UoM {0} => {1}] ", cmc.UoM, updCmc.UoM);
            }

            if (cmc.UseAltDesc != updCmc.UseAltDesc)
            {
                updateString += String.Format(" [UseAltDesc {0} => {1}] ", cmc.UseAltDesc, updCmc.UseAltDesc);
            }

            return updateString;
        }
    }
}