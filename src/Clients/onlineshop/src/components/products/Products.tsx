import { useEffect, useState } from "react";
import ProductCard from "./ProductCard";
import { productServiceApplicationEndpointsProductEndpointsGetProductsEndpoint } from "../../clients/ProductService";
import { ProductServiceContractsQueriesProductProductResponseItem } from "../../clients/ProductService/types.gen";
import LoadingSpinner from "../shared/LoadingSpinner";

type Product = {
    image: string;
} & ProductServiceContractsQueriesProductProductResponseItem;

const Products = () => {
    const [products, setProducts] = useState<Product[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchProducts = async () => {
            try {
                setLoading(true);
                setError(null);
                const productsResp =
                    await productServiceApplicationEndpointsProductEndpointsGetProductsEndpoint();

                if (productsResp.status !== 200) {
                    const errorDetail = productsResp.error
                        ? JSON.stringify(productsResp.error)
                        : `HTTP error ${productsResp.status}`;
                    throw new Error(`Error fetching products: ${errorDetail}`);
                }

                const items = productsResp.data || [];
                const fetchedProducts = items.map((item) => ({
                    ...item,
                    image: `https://picsum.photos/seed/${item.id}/600/600`,
                }));
                setProducts(fetchedProducts);
            } catch (err) {
                console.error("Failed to fetch products:", err);
                setError(
                    err instanceof Error
                        ? err.message
                        : "An unknown error occurred"
                );
            } finally {
                setLoading(false);
            }
        };

        fetchProducts();
    }, []);

    if (loading) {
        return (
            <div className="py-10">
                <LoadingSpinner size="large" />
            </div>
        );
    }

    if (error) {
        return (
            <div className="alert alert-error my-8 max-w-lg mx-auto">
                <p className="font-medium">Error loading products</p>
                <p className="text-sm">{error}</p>
            </div>
        );
    }

    if (products.length === 0) {
        return (
            <div className="text-center py-16 px-4">
                <svg
                    xmlns="http://www.w3.org/2000/svg"
                    className="h-16 w-16 mx-auto text-gray-400 mb-6"
                    fill="none"
                    viewBox="0 0 24 24"
                    stroke="currentColor"
                >
                    <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        strokeWidth={1}
                        d="M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M4 7v10l8 4"
                    />
                </svg>
                <h3 className="text-xl font-medium text-text mb-2">
                    No products found
                </h3>
                <p className="text-text-light">
                    We don't have any products available right now.
                    <br />
                    Please check back later.
                </p>
            </div>
        );
    }

    return (
        <div className="fade-in">
            <div className="mb-8 flex justify-between items-center">
                <h2 className="text-2xl font-semibold text-text">
                    Our Products
                </h2>
                <div className="flex gap-2">
                    <button className="btn btn-outline btn-sm">Filter</button>
                    <select className="form-input text-sm py-1">
                        <option>Latest</option>
                        <option>Price: Low to High</option>
                        <option>Price: High to Low</option>
                        <option>Name</option>
                    </select>
                </div>
            </div>

            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
                {products.map((product) => (
                    <ProductCard key={product.id} product={product} />
                ))}
            </div>
        </div>
    );
};

export default Products;
