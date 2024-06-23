import { useContext, useState, useEffect } from 'react';
import * as signalR from '@microsoft/signalr';
import { Container, Grid, TextField, Button, Select, MenuItem, FormControl, InputLabel, Typography } from '@mui/material';
import { AppUserContext } from "../context/StateContext";
import CrawlerLiveLogs from './CrawlerLiveLogs';

const OrderForm = () => {
  const { appUser } = useContext(AppUserContext);

  const [requestedAmount, setRequestedAmount] = useState('');
  const [productCrawlType, setProductCrawlType] = useState('');
  const [hubConnection, setHubConnection] = useState(null);
  const [isConnected, setIsConnected] = useState(false);

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7010/Hubs/OrderHub')
      .withAutomaticReconnect()
      .build();

    connection.start()
      .then(() => {
        console.log('SignalR connection established.');
        setIsConnected(true);
      })
      .catch(error => {
        console.error('Error establishing SignalR connection:', error);
      });

    setHubConnection(connection);

    return () => {
      connection.stop();
    };
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();

    const orderData = {
      userId: appUser.id,
      email: appUser.email,
      requestedAmount: parseInt(requestedAmount),
      productCrawlType: parseInt(productCrawlType),
    };

    if (hubConnection && isConnected) {
      try {
        await hubConnection.invoke('AddOrder', orderData);
        console.log('Order added successfully');
      } catch (error) {
        console.error('Error adding order:', error);
      }
    } else {
      console.error('Connection is not established. Cannot send order.');
    }
  };

  return (
    <>
    <Container maxWidth="sm">
      <Grid container spacing={2} justifyContent="center" alignItems="center">
        <Grid item xs={12}>
          <Typography variant="h4" align="center" gutterBottom>
            Add Order
          </Typography>
        </Grid>
        <Grid item xs={12}>
          <form onSubmit={handleSubmit}>
            <Grid container spacing={2}>
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  type="number"
                  label="Requested Amount"
                  value={requestedAmount}
                  onChange={(e) => setRequestedAmount(e.target.value)}
                  required
                  InputProps={{ inputProps: { min: 0 } }}
                />
              </Grid>
              <Grid item xs={12}>
                <FormControl fullWidth>
                  <InputLabel id="product-crawl-type-label">Product Crawl Type</InputLabel>
                  <Select
                    labelId="product-crawl-type-label"
                    id="product-crawl-type"
                    value={productCrawlType}
                    onChange={(e) => setProductCrawlType(e.target.value)}
                    required
                  >
                    <MenuItem value="">Select a type</MenuItem>
                    <MenuItem value="0">All</MenuItem>
                    <MenuItem value="1">On Discount</MenuItem>
                    <MenuItem value="2">Non Discount</MenuItem>
                  </Select>
                </FormControl>
              </Grid>
              <Grid item xs={12}>
                <Button
                  type="submit"
                  fullWidth
                  variant="contained"
                  color="primary"
                  disabled={!isConnected}
                >
                  Add Order
                </Button>
              </Grid>
            </Grid>
          </form>
        </Grid>
      </Grid>
    </Container>
    <CrawlerLiveLogs/>
    </>
  );
};

export default OrderForm;
