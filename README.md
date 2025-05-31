# ShopReactDotNetDocker

## Auction Event-Driven Synchronization

This project uses an event-driven architecture to keep the SearchService database in sync with auctions created, updated, or deleted in the AuctionService. MassTransit and RabbitMQ are used for messaging between services.

### 1. Auction Creation, Update, and Deletion Flow

**AuctionService**

- User sends a POST, PUT, or DELETE request to `{{auctionApi}}/api/auctions`.
- AuctionService creates, updates, or deletes an auction in its database.
- AuctionService publishes an event (`AuctionCreatedContract`, `AuctionUpdatedContract`, or `AuctionDeletedContract`) via MassTransit/RabbitMQ.

```
[User]
   |
   v
[POST/PUT/DELETE /api/auctions]
   |
   v
[AuctionService] ---(AuctionCreated/Updated/DeletedContract event)---> [RabbitMQ]
```

### 2. Event Consumption & Search DB Update

**SearchService**

- MassTransit listens for auction events from RabbitMQ.
- `AuctionCreatedConsumer`, `AuctionUpdatedConsumer`, and `AuctionDeletedConsumer` receive the events.
- The consumers map the event to the local `Item` model and create, update, or delete the item in the SearchService MongoDB.

```
[RabbitMQ] ---(AuctionCreated/Updated/DeletedContract event)---> [SearchService]
                                                    |
                                                    v
                        [AuctionCreatedConsumer | AuctionUpdatedConsumer | AuctionDeletedConsumer]
                                                    |
                                                    v
                                         [MongoDB: Item Created/Updated/Deleted]
```

### 3. Search API

**SearchService**

- User sends GET to `{{searchApi}}/api/search?searchTerm=...`
- SearchService queries its MongoDB for matching items.
- Returns search results.

```
[User]
   |
   v
[GET /api/search?searchTerm=...]
   |
   v
[SearchService] --(query MongoDB)--> [Results]
```

### Summary Table

| Step | Component         | Action                                                              |
| ---- | ----------------- | ------------------------------------------------------------------- |
| 1    | AuctionService    | Receives auction create/update/delete, saves to DB, publishes event |
| 2    | RabbitMQ          | Transports event                                                    |
| 3    | SearchService     | Consumes event, creates/updates/deletes item in MongoDB             |
| 4    | SearchService API | User queries search, gets results from MongoDB                      |

---

**Troubleshooting:**

- Ensure AuctionService publishes the correct event after create, update, or delete.
- Ensure SearchService is connected to RabbitMQ and all consumers are active.
- Ensure the consumers save, update, or delete the item in MongoDB as appropriate.
- Ensure the search endpoint queries the correct collection.
