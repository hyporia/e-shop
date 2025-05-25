import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { productServiceApplicationEndpointsProductEndpointsGetProductByIdEndpoint } from "../../clients/ProductService";
import { ProductServiceContractsQueriesProductGetProductByIdResponse } from "../../clients/ProductService/types.gen";
import LoadingSpinner from "../shared/LoadingSpinner";

type Product = {
    image: string;
} & ProductServiceContractsQueriesProductGetProductByIdResponse;

const ProductDetails = () => {
    const { id } = useParams<{ id: string }>();
    const [product, setProduct] = useState<Product | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [quantity, setQuantity] = useState(1);

    useEffect(() => {
        const fetchProduct = async () => {
            if (!id) {
                setError("Product ID is missing.");
                setLoading(false);
                return;
            }

            try {
                setLoading(true);
                setError(null);
                const response =
                    await productServiceApplicationEndpointsProductEndpointsGetProductByIdEndpoint(
                        {
                            path: { id },
                        }
                    );

                if (response.status === 404) {
                    setError("Product not found");
                } else if (response.status !== 200 || !response.data) {
                    const errorDetail = response.error
                        ? JSON.stringify(response.error)
                        : `HTTP error ${response.status}`;
                    setError(`Failed to load product details: ${errorDetail}`);
                } else {
                    setProduct({
                        ...response.data,
                        image: `https://picsum.photos/seed/${id}/1200/800`,
                    });
                }
            } catch (err) {
                console.error("Failed to fetch product:", err);
                setError(
                    err instanceof Error
                        ? err.message
                        : "An unknown error occurred"
                );
            } finally {
                setLoading(false);
            }
        };

        fetchProduct();
    }, [id]);

    // Handle quantity change
    const handleQuantityChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = parseInt(e.target.value);
        if (!isNaN(value) && value > 0) {
            setQuantity(value);
        }
    };

    // Decrement quantity
    const decrementQuantity = () => {
        if (quantity > 1) {
            setQuantity(quantity - 1);
        }
    };

    // Increment quantity
    const incrementQuantity = () => {
        setQuantity(quantity + 1);
    };

    if (loading) {
        return (
            <div className="py-16">
                <LoadingSpinner size="large" />
            </div>
        );
    }

    if (error) {
        return (
            <div className="alert alert-error my-8 max-w-lg mx-auto">
                <p className="font-medium">Error</p>
                <p className="text-sm">{error}</p>
            </div>
        );
    }

    if (!product) {
        return (
            <div className="text-center py-16 px-4">
                <h3 className="text-xl font-medium text-text mb-2">
                    Product Unavailable
                </h3>
                <p className="text-text-light">
                    We couldn't find the product you're looking for.
                </p>
            </div>
        );
    }

    return (
        <div className="fade-in py-8">
            <div className="grid grid-cols-2 md:grid-cols-2 lg:grid-cols-5 gap-8">
                {/* Product Images Section - Takes 3/5 of the grid on large screens */}
                <div className="lg:col-span-3">
                    <div className="bg-gray-50 rounded-xl overflow-hidden">
                        <img
                            src={product.image}
                            alt={product.name || "Product Image"}
                            className="w-full h-auto object-contain aspect-[4/3]"
                        />
                    </div>

                    {/* Additional product images would go here */}
                    <div className="mt-4 grid grid-cols-4 gap-2">
                        {[...Array(4)].map((_, i) => (
                            <div
                                key={i}
                                className="aspect-square bg-gray-100 rounded-lg overflow-hidden cursor-pointer hover:opacity-80 transition-opacity"
                            >
                                <img
                                    src={`https://picsum.photos/seed/${id}-${i}/200/200`}
                                    alt={`Product view ${i + 1}`}
                                    className="w-full h-full object-cover"
                                />
                            </div>
                        ))}
                    </div>
                </div>

                {/* Product Details Section - Takes 2/5 of the grid on large screens */}
                <div className="lg:col-span-2 flex flex-col">
                    <h1 className="text-3xl font-bold text-text mb-2">
                        {product.name}
                    </h1>

                    <div className="text-2xl font-bold text-primary mb-4">
                        ${product.price?.toFixed(2)}
                    </div>

                    {/* Product rating */}
                    <div className="flex items-center mb-4">
                        <div className="flex text-yellow-500">
                            {[...Array(5)].map((_, i) => (
                                <svg
                                    key={i}
                                    xmlns="http://www.w3.org/2000/svg"
                                    viewBox="0 0 24 24"
                                    fill="currentColor"
                                    className="w-5 h-5"
                                >
                                    <path
                                        fillRule="evenodd"
                                        d="M10.788 3.21c.448-1.077 1.976-1.077 2.424 0l2.082 5.006 5.404.434c1.164.093 1.636 1.545.749 2.305l-4.117 3.527 1.257 5.273c.271 1.136-.964 2.033-1.96 1.425L12 18.354 7.373 21.18c-.996.608-2.231-.29-1.96-1.425l1.257-5.273-4.117-3.527c-.887-.76-.415-2.212.749-2.305l5.404-.434 2.082-5.005Z"
                                        clipRule="evenodd"
                                    />
                                </svg>
                            ))}
                        </div>
                        <span className="ml-2 text-text-light text-sm">
                            5.0 (24 reviews)
                        </span>
                    </div>

                    {/* Product description */}
                    <div className="mb-6">
                        <h2 className="text-lg font-medium mb-2">
                            Description
                        </h2>
                        <p className="text-text-light text-sm">
                            {product.description ||
                                "No description available for this product."}
                        </p>
                    </div>

                    {/* Quantity selector */}
                    <div className="mb-6">
                        <h2 className="text-lg font-medium mb-2">Quantity</h2>
                        <div className="flex items-center">
                            <button
                                onClick={decrementQuantity}
                                className="w-10 h-10 rounded-l-md bg-gray-100 flex items-center justify-center hover:bg-gray-200 transition-colors"
                            >
                                <svg
                                    xmlns="http://www.w3.org/2000/svg"
                                    className="h-5 w-5"
                                    fill="none"
                                    viewBox="0 0 24 24"
                                    stroke="currentColor"
                                >
                                    <path
                                        strokeLinecap="round"
                                        strokeLinejoin="round"
                                        strokeWidth={2}
                                        d="M20 12H4"
                                    />
                                </svg>
                            </button>

                            <input
                                type="number"
                                min="1"
                                value={quantity}
                                onChange={handleQuantityChange}
                                className="w-14 h-10 text-center border-t border-b border-gray-200 bg-white"
                            />

                            <button
                                onClick={incrementQuantity}
                                className="w-10 h-10 rounded-r-md bg-gray-100 flex items-center justify-center hover:bg-gray-200 transition-colors"
                            >
                                <svg
                                    xmlns="http://www.w3.org/2000/svg"
                                    className="h-5 w-5"
                                    fill="none"
                                    viewBox="0 0 24 24"
                                    stroke="currentColor"
                                >
                                    <path
                                        strokeLinecap="round"
                                        strokeLinejoin="round"
                                        strokeWidth={2}
                                        d="M12 4v16m8-8H4"
                                    />
                                </svg>
                            </button>
                        </div>
                    </div>

                    {/* Action buttons */}
                    <div className="flex flex-col space-y-3 mt-auto">
                        <button className="btn btn-primary btn-lg">
                            <svg
                                xmlns="http://www.w3.org/2000/svg"
                                className="h-5 w-5 mr-2"
                                fill="none"
                                viewBox="0 0 24 24"
                                stroke="currentColor"
                            >
                                <path
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    strokeWidth={2}
                                    d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z"
                                />
                            </svg>
                            Add to Cart
                        </button>

                        <button className="btn btn-outline">
                            <svg
                                xmlns="http://www.w3.org/2000/svg"
                                className="h-5 w-5 mr-2"
                                fill="none"
                                viewBox="0 0 24 24"
                                stroke="currentColor"
                            >
                                <path
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    strokeWidth={2}
                                    d="M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z"
                                />
                            </svg>
                            Add to Wishlist
                        </button>
                    </div>
                </div>
            </div>

            {/* Product details tabs - Additional info, specs, reviews, etc. */}
            <div className="mt-16">
                <div className="border-b border-gray-200">
                    <nav className="flex space-x-8">
                        {["Description", "Specifications", "Reviews"].map(
                            (tab, i) => (
                                <button
                                    key={i}
                                    className={`py-4 px-1 border-b-2 font-medium text-sm ${
                                        i === 0
                                            ? "border-primary text-primary"
                                            : "border-transparent text-text-light hover:text-text hover:border-gray-300"
                                    }`}
                                >
                                    {tab}
                                </button>
                            )
                        )}
                    </nav>
                </div>

                <div className="py-6">
                    <h3 className="text-xl font-semibold mb-4">
                        Product Details
                    </h3>
                    <p className="text-text-light">
                        {product.description ||
                            "No detailed description available for this product."}
                    </p>
                </div>
            </div>
        </div>
    );
};

export default ProductDetails;
