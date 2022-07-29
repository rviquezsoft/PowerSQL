# PowerSQL

Mini ORM, as powerful in performance as dapper or even better. In queries with large amounts of data it does not consume as much memory as the Entity framework because it uses FastMember instead of datatables. In tests carried out, it was found that with 1,000,000 records, memory consumption and performance were better than with Entity framework, the difference was approximately 1 minute and 30 seconds of difference in performance and approximately 1 GB of difference in memory consumption! !.
