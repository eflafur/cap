using GruppoCap.Core;
using GruppoCap.Core.Data;
using GruppoCap.DAL;
using PetaPoco;
using System;
using System.Collections.Generic;

namespace GruppoCap.Activity.Core
{
    public class ActivityRepo : RepositoryBase<Activity>, IActivityRepo
    {
        // GET LATEST
        public ISubCollection<Activity> Filter(Boolean includePrivileged = false, Int32? companyId = null, Int32 howMany = 1000)
        {
            var sql = Sql.Builder.Append(" SELECT * FROM ( ");
            sql.Append(" SELECT * FROM REVO_ACTIVITY ");

            sql.Append(" WHERE 1 = 1 ");

            if (includePrivileged == false)
                sql.Append(" AND IS_PRIVILEGED = 0 ");

            if (companyId.HasValue)
                sql.Append(" AND COMPANY_ID = @0 ", companyId.Value);

            sql.Append(" ORDER BY MOMENT DESC ");
            sql.Append(" ) WHERE ROWNUM <= {0} ".FormatWith(howMany));
            
            IEnumerable<Activity> _activities = null;
            _activities = db.Query<Activity>(sql);

            return _activities.ToSubCollection<Activity>();
        }

        // FILTER BY ACTOR
        public ISubCollection<Activity> FilterByActor(String actorUserId, Boolean includePrivileged = false, Int32? companyId = null, Boolean upperizeParameters = false, Int32 howMany = 1000)
        {
            return FilterByEntity(
                filterEntityField: "ACTOR",
                filterEntityId: actorUserId,
                includePrivileged: includePrivileged,
                companyId: companyId,
                upperizeParameters: upperizeParameters
            );
        }

        // FILTER BY OBJECT
        public ISubCollection<Activity> FilterByObject(String objectEntityId, String objectEntityType, Boolean includePrivileged = false, Int32? companyId = null, Boolean upperizeParameters = false, Int32 howMany = 1000)
        {
            return FilterByEntity(
                "OBJECT",
                objectEntityId,
                objectEntityType,
                includePrivileged,
                companyId,
                upperizeParameters
            );
        }

        // FILTER BY RELATED
        public ISubCollection<Activity> FilterByRelated(String relatedEntityId, String relatedEntityType, Boolean includePrivileged = false, Int32? companyId = null, Boolean upperizeParameters = false, Int32 howMany = 1000)
        {
            return FilterByEntity(
                "RELATED",
                relatedEntityType,
                relatedEntityId,
                includePrivileged,
                companyId,
                upperizeParameters
            );
        }

        // FILTER BY ENTITY
        private ISubCollection<Activity> FilterByEntity(String filterEntityField, String filterEntityId, String filterEntityType = "", Boolean includePrivileged = false, Int32? companyId = null, Boolean upperizeParameters = false, Int32 howMany = 1000)
        {
            var sql = Sql.Builder.Append(" SELECT * FROM ( ");
            sql = sql.Append(" SELECT * FROM REVO_ACTIVITY ");
            sql.Append(" WHERE ");

            if (filterEntityId.IsNullOrWhiteSpace() == false)
            {
                if (upperizeParameters)
                {
                    sql.Append(" ( UPPER({0}_ENTITY_ID) = @0 ".FormatWith(filterEntityField.ToUpper()), filterEntityId.ToUpper());

                    if (filterEntityType.IsNullOrWhiteSpace() == false)
                    {
                        sql.Append(" AND UPPER({0}_ENTITY_TYPE) = @0 ".FormatWith(filterEntityField.ToUpper()), filterEntityType.ToUpper());
                    }
                }
                else
                {
                    sql.Append(" {0}_ENTITY_ID = @0 ".FormatWith(filterEntityField.ToUpper()), filterEntityId);

                    if (filterEntityType.IsNullOrWhiteSpace() == false)
                    {
                        sql.Append(" AND {0}_ENTITY_TYPE = @0 ".FormatWith(filterEntityField.ToUpper()), filterEntityType);
                    }
                }
            }

            if (includePrivileged == false)
                sql.Append(" AND IS_PRIVILEGED = 0 ");

            if (companyId.HasValue)
                sql.Append(" AND COMPANY_ID = @0 ", companyId.Value);

            sql.Append(" ORDER BY MOMENT DESC ");
            
            sql.Append(" ) WHERE ROWNUM <= {0} ".FormatWith(howMany));

            IEnumerable<Activity> _activities = null;
            _activities = db.Query<Activity>(sql);

            return _activities.ToSubCollection<Activity>();
        }

        // FILTER BY INVOLVED ENTITY
        public ISubCollection<Activity> FilterByInvolvedEntity(String entityId, String entityType, Boolean includePrivileged = false, Int32? companyId = null, Boolean upperizeParameters = false, Int32 howMany = 1000)
        {
            var sql = Sql.Builder.Append(" SELECT * FROM REVO_ACTIVITY ");
            sql.Append(" WHERE ");

            if (entityId.IsNullOrWhiteSpace() == false)
            {
                if (upperizeParameters)
                {
                    sql.Append(" (UPPER(ACTOR_ENTITY_ID) LIKE @0)", "%{1}%".FormatWith(entityId.ToUpper()));

                    sql.Append(" OR ");

                    sql.Append(" (UPPER(IMPERSONATED_ENTITY_ID) LIKE @0)", "%{1}%".FormatWith(entityId.ToUpper()));

                    sql.Append(" OR ");

                    sql.Append(" ((UPPER(OBJECT_ENTITY_ID) LIKE @0)", "%{1}%".FormatWith(entityId.ToUpper()));
                    sql.Append(" AND (UPPER(OBJECT_ENTITY_TYPE) LIKE @0))", "%{1}%".FormatWith(entityType.ToUpper()));

                    sql.Append(" OR ");

                    sql.Append(" ((UPPER(RELATED_ENTITY_ID) LIKE @0)", "%{1}%".FormatWith(entityId.ToUpper()));
                    sql.Append(" AND (UPPER(RELATED_ENTITY_TYPE) LIKE @0))", "%{1}%".FormatWith(entityType.ToUpper()));
                }
                else
                {
                    sql.Append(" (ACTOR_ENTITY_ID LIKE @0)", "%{1}%".FormatWith(entityId.ToUpper()));

                    sql.Append(" OR ");

                    sql.Append(" (IMPERSONATED_ENTITY_ID LIKE @0)", "%{1}%".FormatWith(entityId.ToUpper()));

                    sql.Append(" OR ");

                    sql.Append(" ((OBJECT_ENTITY_ID LIKE @0)", "%{1}%".FormatWith(entityId.ToUpper()));
                    sql.Append(" AND (OBJECT_ENTITY_TYPE LIKE @0))", "%{1}%".FormatWith(entityType.ToUpper()));

                    sql.Append(" OR ");

                    sql.Append(" ((RELATED_ENTITY_ID LIKE @0)", "%{1}%".FormatWith(entityId.ToUpper()));
                    sql.Append(" AND (RELATED_ENTITY_TYPE LIKE @0))", "%{1}%".FormatWith(entityType.ToUpper()));
                }
                    
            }

            if (includePrivileged == false)
                sql.Append(" AND IS_PRIVILEGED = 0 ");

            if (companyId.HasValue)
                sql.Append(" AND COMPANY_ID = @0 ", companyId.Value);

            sql.Append(" ORDER BY MOMENT DESC ");
            sql.Append(" ) WHERE ROWNUM <= {0} ".FormatWith(howMany));

            IEnumerable<Activity> _activities = null;
            _activities = db.Query<Activity>(sql);

            return _activities.ToSubCollection<Activity>();
        }
    }
}
