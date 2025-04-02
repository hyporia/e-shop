import { useEffect, useState } from "react";
import "./Products.css";
import { getProducts } from "../../clients/ProductService";
import { ProductResponseItem } from "../../clients/ProductService/types.gen";

type Product = {
    image: string;
} & ProductResponseItem;

const Products = () => {
    const [products, setProducts] = useState<Product[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchProducts = async () => {
            try {
                setLoading(true);
                const productsResp = await getProducts();
                if (productsResp.status !== 200) {
                    throw new Error(
                        `Error fetching products: ${productsResp.error}`
                    );
                }
                const items = productsResp.data?.products || [];
                const products = items.map((item) => ({
                    ...item,
                    image: "https://picsum.photos/300",
                })); // Example image URL
                setProducts(products);
            } catch (error) {
                console.error("Failed to fetch products:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchProducts();
    }, []);

    return (
        <div className="products-container">
            <h1>Our Products</h1>
            {loading ? (
                <p>Loading products...</p>
            ) : (
                <div className="products-grid">
                    {products.map((product) => (
                        <div key={product.id} className="product-card">
                            <div className="product-image">
                                <img src={product.image} alt={product.name} />
                            </div>
                            <div className="product-details">
                                <h3>{product.name}</h3>
                                <p className="product-price">
                                    ${product.price.toFixed(2)}
                                </p>
                                <p className="product-description">
                                    {product.description}
                                </p>
                                <button className="add-to-cart">
                                    Add to Cart
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};

export default Products;
