# Kafka Order Processing Example

## Overview
This example demonstrates the complete Kafka-based order processing flow in this microservice.

## Prerequisites
1. **Kafka and Zookeeper running**:
   ```bash
   docker ps | grep kafka
   ```
   Should show containers on ports 9092 (Kafka) and 2181 (Zookeeper)

2. **Application running**:
   ```bash
   cd /Users/Prashant_Kumar2/dotnetcore-kafka-integration-2
   dotnet run --project Api/Api.csproj
   ```

## Example 1: Create a Simple Order

**Request:**
```bash
curl -X POST http://localhost:5000/api/order \
  -H "Content-Type: application/json" \
  -d '{
    "productname": "Laptop",
    "quantity": 2,
    "amount": 1500.00
  }'
```

**Response:**
```
Your order is in progress
```

**What happens:**
1. API receives the POST request
2. Order is published to Kafka topic `orderrequests`
3. Background service (`ProcessOrdersService`) consumes the message
4. Order status is updated to `COMPLETED`
5. Processed order is published to Kafka topic `readytoship`

---

## Example 2: Multiple Orders

**Create multiple orders:**
```bash
# Order 1 - Electronics
curl -X POST http://localhost:5000/api/order \
  -H "Content-Type: application/json" \
  -d '{
    "productname": "Wireless Mouse",
    "quantity": 5,
    "amount": 125.00
  }'

# Order 2 - Office Supplies
curl -X POST http://localhost:5000/api/order \
  -H "Content-Type: application/json" \
  -d '{
    "productname": "Notebook Pack",
    "quantity": 10,
    "amount": 50.00
  }'

# Order 3 - Software
curl -X POST http://localhost:5000/api/order \
  -H "Content-Type: application/json" \
  -d '{
    "productname": "Software License",
    "quantity": 1,
    "amount": 299.99
  }'
```

---

## Example 3: Testing with Different Order Amounts

**Small order:**
```bash
curl -X POST http://localhost:5000/api/order \
  -H "Content-Type: application/json" \
  -d '{
    "productname": "USB Cable",
    "quantity": 1,
    "amount": 9.99
  }'
```

**Large order:**
```bash
curl -X POST http://localhost:5000/api/order \
  -H "Content-Type: application/json" \
  -d '{
    "productname": "Server Rack",
    "quantity": 3,
    "amount": 15000.00
  }'
```

---

## Monitoring Order Processing

**Check application logs:**
```bash
# You should see output like:
# OrderProcessing Service Started
# Info: OrderHandler => Processing the order for Laptop
# Info: OrderHandler => Processing the order for Wireless Mouse
```

**Monitor Kafka topics using kafka-console-consumer:**
```bash
# Monitor incoming orders
docker exec -it <kafka-container-id> kafka-console-consumer \
  --bootstrap-server localhost:9092 \
  --topic orderrequests \
  --from-beginning

# Monitor processed orders
docker exec -it <kafka-container-id> kafka-console-consumer \
  --bootstrap-server localhost:9092 \
  --topic readytoship \
  --from-beginning
```

---

## Data Model

**OrderRequest Model:**
```csharp
public class OrderRequest
{
    public string productname { get; set; }
    public decimal amount { get; set; }
    public int quantity { get; set; }
    public OrderStatus status { get; set; }
}

public enum OrderStatus
{
    INPROGRESS,
    COMPLETED
}
```

**Example JSON:**
```json
{
  "productname": "Laptop",
  "amount": 1500.00,
  "quantity": 2,
  "status": 0
}
```

After processing, status changes to `1` (COMPLETED).

---

## Architecture Flow

```
┌─────────────┐
│   Client    │
└──────┬──────┘
       │ POST /api/order
       ▼
┌─────────────────────┐
│  OrderController    │
└──────┬──────────────┘
       │ Publishes to Kafka
       ▼
┌─────────────────────┐
│ Topic: orderrequests│
└──────┬──────────────┘
       │ Consumed by
       ▼
┌──────────────────────────┐
│ ProcessOrdersService     │ (Background Service)
│ - Reads message          │
│ - Processes order        │
│ - Updates status         │
└──────┬───────────────────┘
       │ Publishes to Kafka
       ▼
┌─────────────────────┐
│ Topic: readytoship  │
└─────────────────────┘
```

---

## Troubleshooting

**Connection refused:**
- Ensure Kafka is running: `docker ps`
- Check Kafka is on port 9092: `lsof -i :9092`

**Port 5000 already in use:**
```bash
lsof -ti:5000 | xargs kill -9
```

**Application crashes:**
- Check logs for null reference errors
- Verify Kafka topics exist
- Ensure configuration in `appsettings.json` is correct

---

## Testing Tips

1. **Use different product names** to track orders through the system
2. **Monitor console output** to see processing messages
3. **Check both Kafka topics** to verify end-to-end flow
4. **Vary quantities and amounts** to test different scenarios
5. **Send rapid requests** to test concurrent processing
