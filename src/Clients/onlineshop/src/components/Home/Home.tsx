import "./Home.css";
import Products from "../products/Products";

const Home = (): JSX.Element => {
    return (
        <div className="home">
            <header className="home-header">
                <h1>Welcome to the Online Shop</h1>
            </header>
            <main className="home-content">
                <p>
                    Discover our wide range of products and enjoy shopping with
                    us!
                </p>
                <Products />
            </main>
        </div>
    );
};

export default Home;
