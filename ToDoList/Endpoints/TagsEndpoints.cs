using ToDoList.Dtos;
using ToDoList.Infrastructure.Data;
using ToDoList.Services;
using ToDoList.Shared.Constants;
using ToDoList.Shared.Extensions;
using ToDoList.Shared.Mappings;

namespace ToDoList.Endpoints;

public static class TagsEndpoints
{
    public static void MapTagsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tags").RequireAuthorization().RequireUserContext();

        group.MapPost("", async (TagRequest tagRequest, TagsService tagsService, HttpContext context, CancellationToken cancellationToken) =>
        {
            var tag = await tagsService.Create(tagRequest.ToTagModel(context.GetUserId()), cancellationToken);
            return Results.Ok(tag.ToResponse());
        }).Validate<TagRequest>().WithName(ApiEndpointNames.CreateTag);

        group.MapGet("", async (TagsService tagsService, HttpContext context, CancellationToken cancellationToken) =>
        {
            var tags = await tagsService.GetAll(context.GetUserId(), cancellationToken);
            var response = tags.Select(t => t.ToResponse());
            return Results.Ok(response);
        }).WithName(ApiEndpointNames.GetAllTags);

        var concreteTagGroup = group.MapGroup("/{id:guid}").AddEndpointFilter(async (context, next) =>
        {
            var userId = context.HttpContext.GetUserId();
            var tagId = context.Arguments.OfType<Guid>().FirstOrDefault();
            var db = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();

            var tag = await db.Tags.FindAsync(new object?[] { tagId }, context.HttpContext.RequestAborted);
            if (tag == null || tag.UserId != userId)
            {
                return Results.NotFound();
            }

            return await next(context);
        });

        concreteTagGroup.MapGet("", async (Guid id, TagsService tagsService, CancellationToken cancellationToken) =>
        {
            var tag = await tagsService.Get(id, cancellationToken);
            return tag == null ? Results.NotFound() : Results.Ok(tag.ToResponse());
        }).WithName(ApiEndpointNames.GetTag);
        // TODO: Add delete tag feature on frontend
        // concreteTagGroup.MapDelete("", async (Guid id, TagsService tagsService, CancellationToken cancellationToken) =>
        // {
        //     await tagsService.Delete(id, cancellationToken);
        //     return Results.NoContent();
        // }).WithName(ApiEndpointNames.DeleteTag);;
    }
}
