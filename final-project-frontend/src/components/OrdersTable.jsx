import { 
  Button,
  Table, 
  TableHead, 
  TableBody, 
  TableRow, 
  TableCell, 
  CircularProgress, 
  Alert, 
  Typography,
  Container,
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions
} from '@mui/material';
import { Visibility } from '@mui/icons-material'; 
import axios from 'axios';
import { useEffect, useState, useContext } from 'react';
import { AppUserContext } from "../context/StateContext";
import * as XLSX from 'xlsx'; 

const BASE_URL = import.meta.env.VITE_API_URL;

const OrdersTable = () => {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedOrder, setSelectedOrder] = useState(null);
  const [products, setProducts] = useState([]);
  const [events, setEvents] = useState([]);
  const [productsLoading, setProductsLoading] = useState(false);
  const [eventsLoading, setEventsLoading] = useState(false);
  const [productsError, setProductsError] = useState(null);
  const [eventsError, setEventsError] = useState(null);
  const [viewingType, setViewingType] = useState(null); 

  const { appUser } = useContext(AppUserContext);

  useEffect(() => {
    const fetchOrders = async () => {
      try {
        const response = await axios.post(`${BASE_URL}/Orders/GetByUserId`, { userId: appUser.id }, {
          headers: {
            'Content-Type': 'application/json',
          },
        });

        const sortedOrders = response.data.sort((a, b) => new Date(b.createdOn) - new Date(a.createdOn));
        setOrders(sortedOrders);
      } catch (error) {
        setError(error.message);
      } finally {
        setLoading(false);
      }
    };

    if (appUser.id) {
      fetchOrders();
    }
  }, [appUser.id]);

  const orderStatusMap = {
    1: 'Bot Started',
    2: 'Crawling Started',
    3: 'Crawling Completed',
    4: 'Crawling Failed',
    5: 'Order Completed'
  };
  
  const getOrderStatusText = (statusCode) => {
    return orderStatusMap[statusCode] || 'Unknown Status';
  }
  

  const getProductCrawlTypeText = (type) => {
    switch (type) {
      case 0:
        return 'All';
      case 1:
        return 'On Discount';
      case 2:
        return 'Non Discount';
      default:
        return 'Unknown';
    }
  };

  const handleViewProducts = async (orderId) => {
    setProductsLoading(true);
    setProductsError(null);
    setSelectedOrder(orderId);
    setViewingType('products');
    try {
    //  const response = await axios.post(`${BASE_URL}/Products/GetByOrderId/${orderId}`);

      const response = await axios.post(`${BASE_URL}/Products/GetByOrderId`, { orderId: orderId }, {
        headers: {
          'Content-Type': 'application/json',
        },
      });
      setProducts(response.data);
    } catch (error) {
      setProductsError(error.message);
    } finally {
      setProductsLoading(false);
    }
  };

  const handleViewEvents = async (orderId) => {
    setEventsLoading(true);
    setEventsError(null);
    setSelectedOrder(orderId);
    setViewingType('events');
    try {
      const response = await axios.post(`${BASE_URL}/OrderEvents/GetByOrderId/`, { orderId: orderId });
      setEvents(response.data);
    } catch (error) {
      setEventsError(error.message);
    } finally {
      setEventsLoading(false);
    }
  };

  const handleCloseDialog = () => {
    setSelectedOrder(null);
    setProducts([]);
    setEvents([]);
    setProductsError(null);
    setEventsError(null);
    setViewingType(null);
  };

  const exportProductsToExcel = () => {
    const worksheet = XLSX.utils.json_to_sheet(products);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, "Products");
    XLSX.writeFile(workbook, `order_${selectedOrder}_products.xlsx`);
  };

  if (loading) {
    return (
      <Container sx={{ display: 'flex', justifyContent: 'center', mt: 5 }}>
        <CircularProgress />
      </Container>
    );
  }

  if (error) {
    return (
      <Container sx={{ mt: 5 }}>
        <Alert severity="error">{error}</Alert>
      </Container>
    );
  }

  return (
    <Container sx={{ mt: 5 }}>
      <Typography variant="h4" component="h1" gutterBottom>
        Orders
      </Typography>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>Order ID</TableCell>
            <TableCell>Created On</TableCell>
            <TableCell>Requested Amount</TableCell>
            <TableCell>Product Crawl Type</TableCell>
            <TableCell>Total Found Amount</TableCell>
            <TableCell>Products</TableCell>
            <TableCell>Events</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {orders.map((order) => (
            <TableRow key={order.id}>
              <TableCell>{order.id}</TableCell>
              <TableCell>{new Date(order.createdOn).toLocaleString()}</TableCell>
              <TableCell>{order.requestedAmount}</TableCell>
              <TableCell>{getProductCrawlTypeText(order.productCrawlType)}</TableCell>
              <TableCell>{order.totalFoundAmount}</TableCell>
              <TableCell>
                <IconButton color="primary" onClick={() => handleViewProducts(order.id)} title="View Products">
                  <Visibility />
                </IconButton>
              </TableCell>
              <TableCell>
                <IconButton color="primary" onClick={() => handleViewEvents(order.id)} title="View Events">
                  <Visibility />
                </IconButton>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>

      <Dialog open={selectedOrder !== null} onClose={handleCloseDialog} fullWidth maxWidth="md">
        <DialogTitle>Details for Order {selectedOrder}</DialogTitle>
        <DialogContent>
          {viewingType === 'products' ? (
            productsLoading ? (
              <CircularProgress />
            ) : productsError ? (
              <Alert severity="error">{productsError}</Alert>
            ) : (
              <div>
                <Typography variant="h6" component="h2" gutterBottom>
                  Products
                </Typography>
                <Button 
                    variant="contained" 
                    color="primary" 
                    onClick={exportProductsToExcel} 
                    sx={{ mb: 2 }}
                  >
                    Export to Excel
                  </Button>
                <Table>
                  <TableHead>
                    <TableRow>
                      <TableCell>Product ID</TableCell>
                      <TableCell>Name</TableCell>
                      <TableCell>Price</TableCell>
                      <TableCell>Is On Sale</TableCell>
                    </TableRow>
                  </TableHead>
                  <TableBody>
                    {products.map((product) => (
                      <TableRow key={product.id}>
                        <TableCell>{product.id}</TableCell>
                        <TableCell>{product.name}</TableCell>
                        <TableCell>{product.price}</TableCell>
                        <TableCell>{product.isOnSale ? 'Yes' : 'No'}</TableCell>
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>
              </div>
            )
          ) : viewingType === 'events' ? (
            eventsLoading ? (
              <CircularProgress />
            ) : eventsError ? (
              <Alert severity="error">{eventsError}</Alert>
            ) : (
              <div style={{ marginTop: '20px' }}>
                <Typography variant="h6" component="h2" gutterBottom>
                  Events
                </Typography>
                <Table>
                  <TableHead>
                    <TableRow>
                      <TableCell>Date</TableCell>
                      <TableCell>Status</TableCell>
                    </TableRow>
                  </TableHead>
                  <TableBody>
                    {events.map((event) => (
                      <TableRow key={event.id}>
                        <TableCell>{new Date(event.createdOn).toLocaleString()}</TableCell>
                        <TableCell>{getOrderStatusText(event.status)}</TableCell>
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>
              </div>
            )
          ) : null}
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseDialog} color="primary">
            Close
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};

export default OrdersTable;
