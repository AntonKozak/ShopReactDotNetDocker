# ShopReactDotNetDocker

## Auction Creation and Search Propagation Flow

This project uses an event-driven architecture to keep the SearchService database in sync with new auctions created in the AuctionService. Here’s how the process works:

### 1. Auction Creation Flow

**AuctionService**

- User sends a POST request to `{{auctionApi}}/api/auctions`.
- AuctionService creates a new auction in its database.
- AuctionService publishes an `AuctionCreatedContract` event (via MassTransit/RabbitMQ).

```
[User]
   |
   v
[POST /api/auctions]
   |
   v
[AuctionService] ---(AuctionCreatedContract event)---> [RabbitMQ]
```

### 2. Event Consumption & Search DB Update

**SearchService**

- MassTransit listens for `AuctionCreatedContract` events from RabbitMQ.
- `AuctionCreatedConsumer` receives the event.
- Maps the event to a local `Item` model.
- Saves the new item to the SearchService MongoDB.

```
[RabbitMQ] ---(AuctionCreatedContract event)---> [SearchService]
                                                    |
                                                    v
                                    [AuctionCreatedConsumer]
                                                    |
                                                    v
                                         [MongoDB: Item Saved]
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

| Step | Component         | Action                                                  |
| ---- | ----------------- | ------------------------------------------------------- |
| 1    | AuctionService    | Receives auction creation, saves to DB, publishes event |
| 2    | RabbitMQ          | Transports event                                        |
| 3    | SearchService     | Consumes event, saves item to MongoDB                   |
| 4    | SearchService API | User queries search, gets results from MongoDB          |

---

**Troubleshooting:**

- Ensure AuctionService publishes the event after creation.
- Ensure SearchService is connected to RabbitMQ and the consumer is active.
- Ensure the consumer saves the item to MongoDB.
- Ensure the search endpoint queries the correct collection.
