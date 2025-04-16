import { useEffect, useState } from "react";
import ProductCard from "./ProductCard";
import { productServiceApplicationEndpointsProductEndpointsGetProductsEndpoint } from "../../clients/ProductService";
import { ProductServiceApplicationQueriesProductProductResponseItem } from "../../clients/ProductService/types.gen";

type Product = {
    image: string;
} & ProductServiceApplicationQueriesProductProductResponseItem;

const Products = () => {
    const [products, setProducts] = useState<Product[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchProducts = async () => {
            try {
                setLoading(true);
                const productsResp =
                    await productServiceApplicationEndpointsProductEndpointsGetProductsEndpoint();
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
        <div className="container p-lg">
            <h1 className="text-3xl font-bold text-center mb-xl">
                Our Products
            </h1>
            {loading ? (
                <p className="text-center">Loading products...</p>
            ) : (
                <div className="grid grid-cols-products gap-lg">
                    {products.map((product) => (
                        <ProductCard key={product.id} product={product} />
                    ))}
                </div>
            )}
        </div>
    );
};

export default Products;
